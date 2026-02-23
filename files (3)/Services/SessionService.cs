using EventEase.Models;
using System;
using System.Collections.Generic;

namespace EventEase.Services
{
    public class SessionService
    {
        public string? UserName { get; private set; }
        public string? UserEmail { get; private set; }
        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserEmail);

        private readonly List<Registration> _userRegistrations = new();

        public event Action? OnSessionChanged;

        public void StartSession(string name, string email)
        {
            UserName  = name;
            UserEmail = email;
            OnSessionChanged?.Invoke();
        }

        public void EndSession()
        {
            UserName  = null;
            UserEmail = null;
            _userRegistrations.Clear();
            OnSessionChanged?.Invoke();
        }

        public void AddRegistration(Registration reg)
        {
            _userRegistrations.Add(reg);
            OnSessionChanged?.Invoke();
        }

        public IReadOnlyList<Registration> GetUserRegistrations() =>
            _userRegistrations.AsReadOnly();
    }
}