using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using ReChat.Data.EntityFrameworkCore;
using ReChat.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReChat.Data.Extensions
{
   public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Extensions/SeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                }
            }
        }
    }
}
