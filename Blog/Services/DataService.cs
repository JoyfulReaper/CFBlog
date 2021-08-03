﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCBlog.Data;
using MVCBlog.Enums;
using MVCBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<BlogUser> userManager )
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            await _context.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if(_context.Roles.Any())
            {
                // DB already seeded
                return;
            }

            foreach(var role in Enum.GetNames(typeof(BlogRole)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            if(_context.Users.Any())
            {
                // Already seeded
                return;
            }

            var adminUser = new BlogUser()
            {
                Email = "website@kgivler.com",
                UserName = "website@kgivler.com",
                FirstName = "Kyle",
                LastName = "Givler",
                DisplayName = "Kyle Givler",
                GitHubUrl = "https://github.com/JoyfulReaper",
                PersonalUrl = "https://kgivler.com",
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(adminUser, "Password123!");
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());

            var modUser = new BlogUser()
            {
                Email = "websitem@kgivler.com",
                UserName = "websitem@kgivler.com",
                FirstName = "Kyle",
                LastName = "Givler",
                DisplayName = "Kyle Givler",
                GitHubUrl = "https://github.com/JoyfulReaper",
                PersonalUrl = "https://kgivler.com",
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(modUser, "Password123!");
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());
        }
    }
}
