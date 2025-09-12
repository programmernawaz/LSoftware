﻿namespace PathLabAPI.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; }= string.Empty;
        public string DisplayName { get; set; }
    }
}
