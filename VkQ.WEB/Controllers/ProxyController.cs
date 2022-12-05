﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VkQ.Application.Abstractions.Proxies.ServicesInterfaces;
using VkQ.WEB.ViewModels.Proxy;

namespace VkQ.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProxyController : Controller
    {
        private readonly IProxyManager _proxyManager;

        public ProxyController(IProxyManager proxyManager)
        {
            _proxyManager = proxyManager;
        }

        [HttpGet]
        public IActionResult Index(string message)
        {
            if (!string.IsNullOrEmpty(message)) ViewData["Alert"] = message;
            var proxies = await _proxyManager.GetProxiesAsync();
            return View(new AddProxyViewModel()
                { Proxies =  });
        }
        
        public IActionResult DeleteProxy(int id)
        {
            var proxy = _db.Proxies.Include(proxy1 => proxy1.Instagrams).FirstOrDefault(proxy1 => proxy1.Id == id);
            if (proxy == null)
                return RedirectToAction("Index",
                    new
                    {
                        message = "Прокси не найдена."
                    });
            return _proxyService.DeleteProxy(proxy)
                ? RedirectToAction("Index",
                    new
                    {
                        message = "Прокси успешно удалена."
                    })
                : RedirectToAction("Index",
                    new
                    {
                        message = "Не удалось удалить прокси."
                    });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AddProxyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Proxies = _db.Proxies.OrderByDescending(proxy => proxy.Id).ToList();
                return RedirectToAction("Index", model);
            }

            if (_proxyService.AddProxy(model.ProxyList))
            {
                ViewData["Alert"] = "Прокси успешно загружены.";
                return View(new AddProxyViewModel { Proxies = _db.Proxies.OrderByDescending(proxy => proxy.Id).ToList() });
            }

            ModelState.AddModelError("", "Список прокси введён некорректно.");
            model.Proxies = _db.Proxies.OrderByDescending(proxy => proxy.Id).ToList();
            return View(model);
        }
    }
}