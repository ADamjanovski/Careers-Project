using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalProject.Models.Validation
{
    public class IsImageValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase; 
            if (file != null || (file.ContentType!="image/jpeg" && file.ContentType!="image/png")) {
                return false;
            }
            if (file.ContentLength > 2 * 1024 * 1024)
            {
                return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Only jpeg/png files are allowed and they can't exceed 2MB";
        }
    }
}