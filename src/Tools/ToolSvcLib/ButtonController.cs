using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcLib.Models;

namespace ToolSvcLib
{
    public class ButtonController
    {
        public bool Disabled { get; set; }
        public string? Content { get; set; }
        public string? CssClass { get; set; }
        public ButtonModel BtnDef { get; set; }

        //public ButtonController(ButtonModel btnModel)
        //{
        //    BtnDef = btnModel;
        //    Disable(); //default state
        //}

        public ButtonController()
        {
            BtnDef = new ButtonModel();
        }

        public void Disable ()
        {
            Disabled = true;
            Content = BtnDef.DisabledContent;
            CssClass = BtnDef.DisabledCssClass;
        }
        
        public void Enable()
        {
            Disabled = false;
            Content = BtnDef.EnabledContent;
            CssClass = BtnDef.EnabledCssClass;
        }

        public void InProcess()
        {
            Disabled = true;
            Content = BtnDef.InProcessContent;
            CssClass = BtnDef.InProcessCssClass;
        }
        
        public void Success()
        {
            Disabled = true;
            Content = BtnDef.SuccessContent;
            CssClass = BtnDef.SuccessCssClass;
        }
        
        public void Failed()
        {
            Disabled = true;
            Content = BtnDef.FailedContent;
            CssClass = BtnDef.FailedCssClass;
        }
        
        public void Success(string content)
        {
            Disabled = true;
            Content = content;
            CssClass = BtnDef.SuccessCssClass;
        }
        
        public void Failed(string content)
        {
            Disabled = true;
            Content = content;
            CssClass = BtnDef.FailedCssClass;
        }

        public void CustomEnable(string content)
        {
            Disabled = false;
            Content = content;
            CssClass = BtnDef.EnabledCssClass;
        }

        public void CustomEnable(string content, string cssClass)
        {
            Disabled = false;
            Content = content;
            CssClass = cssClass;
        }

        public void Custom(bool disabled, string content, string cssClass)
        {
            Disabled = disabled;
            Content = content;
            CssClass = cssClass;
        }

    }
}
