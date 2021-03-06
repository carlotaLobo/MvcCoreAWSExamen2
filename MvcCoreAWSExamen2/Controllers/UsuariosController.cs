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
        public async Task<IActionResult> Create(String masinfo, Usuario usuario, List<IFormFile> foto, List<String> titulo)
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
                        usuario.Fotos.Add(new Foto { Titulo = titulo[i], Imagen = this.services3.GetUrlFile(foto[i].FileName) });
                    }

                }
            }
            await this.servicedynamo.Create(usuario);

            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(int id)
        {

            Usuario usu = await this.servicedynamo.GetUsuario(id);
            if (usu.Fotos.Count > 0)
            {
                foreach (Foto u in usu.Fotos)
                {
                    String imagen = u.Imagen.Substring(u.Imagen.IndexOf(".com/") + 1, u.Imagen.Length - u.Imagen.IndexOf(".com/") - 1);
                    await this.services3.DeleteFile(imagen);
                }
            }
            await this.servicedynamo.Delete(id);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(await this.servicedynamo.GetUsuario(id));
        }
        [HttpPost]
        public async Task<IActionResult> Update(String masinfo, Usuario usuario, List<IFormFile> foto, List<String> titulo)
        {
            Usuario u = await this.servicedynamo.GetUsuario(usuario.IdUsuario);
            if (masinfo != null)
            {
                if (foto != null)
                {
                    if (u.Fotos !=null)
                    {
                        usuario.Fotos = u.Fotos;
                    }
                    else
                    {
                        usuario.Fotos = new List<Foto>();
                    }

                       for (int i = 0; i < foto.Count; i++)
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                foto[i].CopyTo(m);
                                await this.services3.UploadFile(m, foto[i].FileName);
                            }
                            usuario.Fotos.Add(new Foto { Titulo = titulo[i], Imagen = this.services3.GetUrlFile(foto[i].FileName) });
                        }
                    }
                }

                await this.servicedynamo.Update(usuario);

                return View(await this.servicedynamo.GetUsuario(usuario.IdUsuario));
            }
            public async Task<IActionResult> DeleteFoto(int idusuario, int posicion)
            {
                await this.servicedynamo.DeleteImagen(idusuario, posicion);
                return RedirectToAction("index");
            }
        }
    }
