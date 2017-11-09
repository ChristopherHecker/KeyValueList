// (c) 2017 Chris Hecker.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using KeyListWebApp.Models;
using KeyValueList;

namespace KeyListWebApp.Controllers
{
    public class HomeController : Controller
    {
        private static KeyValueList<string, string> keyValueList = new KeyValueList<string, string>();

        public IActionResult Index()
        {
            ViewData["Message"] = "Your kvview home page.";
            var model = new KVmodel(keyValueList.DisplayList);
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "KeyListView 1.0";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Add(KVmodel kvmodel)
        {
            if (kvmodel.Text != null)
            {
                string[] values = kvmodel.Text.Split('=');
                if (values.Length == 2)
                {
                    keyValueList.AddPair(values[0], values[1]);
                }
                else
                {
                    string text = "Bad format: item must consist of a single key value pair separated by '='";
                    return RedirectToAction("Error", new ErrorViewModel { Text = text });
                }
            }
            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        public IActionResult Clear()
        {
            keyValueList.ClearAll();
            var model = new KVmodel(keyValueList.DisplayList);

            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public IActionResult RemoveSelected(KVmodel kvmodel)
        {
            foreach (string s in kvmodel.Strings)
            {
                string[] values = s.Split('=');
                if (values.Length == 2)
                {
                    keyValueList.RemovePair(values[0], values[1]);
                }
                else
                {
                    string text = "Bad format: item to be removed must consist of a single key value pair separated by '='";
                    return RedirectToAction("Error", new ErrorViewModel { Text = text });
                }
            }
            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public IActionResult ExportToJson(KVmodel kvmodel)
        {
            try
            {
                keyValueList.SaveAsJson(kvmodel.Text);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", new ErrorViewModel { Text = ex.Message });
            }

            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public IActionResult ExportToXml(KVmodel kvmodel)
        {
            try
            {
                keyValueList.SaveAsXml(kvmodel.Text);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new ErrorViewModel { Text = ex.Message });
            }

            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        public IActionResult SortByName()
        {
            keyValueList.SortByKey();

            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        public IActionResult SortByValue()
        {
            keyValueList.SortByValue();

            var model = new KVmodel(keyValueList.DisplayList);
            return RedirectToAction("Index", model);
        }

        public IActionResult Error(ErrorViewModel evm)
        {
            return View(evm);
        }
    }
}
