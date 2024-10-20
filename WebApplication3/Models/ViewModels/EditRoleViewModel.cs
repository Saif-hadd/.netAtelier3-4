﻿using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace WebApplication3.Models.ViewModels
{
public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

    }
}
