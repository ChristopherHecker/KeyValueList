using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KeyListWebApp.Models
{
    public class KVmodel
    {
        public SelectList Items { get; set; }
        public List<string> Strings { get; set; }
        public string Text { get; set; }

        private void UnselectList()
        {
            foreach (var item in Items)
            {
                item.Selected = false;
            }
        }
        public KVmodel(List<string> items)
        {
            Strings = new List<string>(items);
            Items = new SelectList(items);
            UnselectList();
        }
        public KVmodel()
        {
            Strings = new List<string>();
            Items = new SelectList(Strings);
            UnselectList();
        }
    }
}
