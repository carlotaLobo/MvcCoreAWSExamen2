using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSExamen2.Models;
using MvcCoreAWSExamen2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSExamen2.Controllers
{
    public class UsuariosController : Controller
    {
        ServiceDynamoDb servicedynamo;
        ServiceS3 services3;

        public UsuariosController(ServiceDynamoDb servicedynamo, ServiceS3 services3)
        {
            this.servicedynamo = servicedynamo;
            this.services3 = services3;
        }
        public async Task<IActionResult> Index()
        {
            return View(await this.servicedynamo.GetUsuarios());
        }
        public async Task<IActionResult> Details(int id)
        {
            return View(await this.servicedynamo.GetUsuario(id));
        }
       
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(String masinfo, Usuario usuario, List<IFormFile> foto, String titulo)
        {
            if (masinfo != null)
            {
                if (foto != null)
                {
                    usuario.Fotos = new List<Foto>();

                    for (int i = 0; i < foto.Count; i++)
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            foto[i].CopyTo(m);
                            await this.services3.UploadFile(m, foto[i].FileName);
                        }
                        usuario.Fotos.Add(new Foto { Titulo = titulo, Imagen = this.services3.GetUrlFile(foto[i].FileName) });
                    }

                }
            }
            await this.servicedynamo.Create(usuario);

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int idusuario)
        {
            await this.servicedynamo.Delete(idusuario);
            Usuario usu = await this.servicedynamo.GetUsuario(idusuario);
            if (usu.Fotos.Count > 0)
            {
                foreach (Foto u in usu.Fotos)
                {
                    String imagen = u.Imagen.Substring(u.Imagen.IndexOf(".com/") + 1, u.Imagen.Length - u.Imagen.IndexOf(".com/") - 1);     
                    await this.services3.DeleteFile(imagen);
                }
            }

            return RedirectToAction("index");
        }
    }
}
