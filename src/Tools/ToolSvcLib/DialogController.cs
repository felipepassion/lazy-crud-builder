using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcLib
{
    public class DialogController
    {
        public bool Visible { get; set; }
        public string? Content { get; set; }
        public string? Header { get; set; }
        public string? NavigationPath { get; set; }
        public bool? YesNo { get; set; }

        public DialogController()
        {
            Visible = false;
            Header = string.Empty;
            Content = string.Empty;
            NavigationPath = string.Empty;

        }

        public DialogController(string? header)
        {
            Visible = false;
            Header = header;
            Content = string.Empty;
            NavigationPath = string.Empty;
        }

        public DialogController(string? header, string? content )
        {
            Visible = false;
            Header = header;
            Content = content;
            NavigationPath = string.Empty;
        }

        public DialogController(string? header, string? content, string? navPath)
        {
            Visible=false;
            Header = header;
            Content = content;
            NavigationPath = navPath;
        }

        public void Display()
        {
            Visible = true;
        }

        public void Display(string? content)
        {
            Visible = true;
            Content = content;
        }

        public void Display(string? header, string? content)
        {
            Visible = true;
            Header = header;
            Content = content;
        }

        public void Display(string? header, string? content,string? navPath)
        {
            Visible = true;
            Header = header;
            Content = content;
            NavigationPath=navPath;
        }

    }
}
