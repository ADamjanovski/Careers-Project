using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PersonalProject.Models.Validation
{
    public class MacedonianPhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string phoneNumber = value as string;

            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false; // Consider empty values as valid if necessary
            }

            // If the phone number starts with '0', replace it with '+389'
            if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = "+389" + phoneNumber.Substring(1);
            }

            // Validate phone number using the international format regex
            var regex = new Regex(@"^\+389\d{8}$"); // Validates +389 followed by 8 digits
            return regex.IsMatch(phoneNumber);
        }

        public override string FormatErrorMessage(string name)
        {
            return "Please enter a valid Macedonian phone number in the international format (e.g. +38912345678) or local format (e.g. 071234567).";
        }
    }
}