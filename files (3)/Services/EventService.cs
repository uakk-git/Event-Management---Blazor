using EventEase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventEase.Services
{
    public class EventService
    {
        private readonly List<Event> _events = new()
        {
            new Event { Id = 1, Name = "Tech Conference 2026", Date = new DateTime(2026, 4, 10), Location = "New York, NY", Description = "Annual technology conference.", Capacity = 300, AttendeesCount = 120 },
            new Event { Id = 2, Name = "Marketing Summit",     Date = new DateTime(2026, 5, 15), Location = "Chicago, IL",  Description = "Summit for marketing professionals.", Capacity = 150, AttendeesCount = 90 },
            new Event { Id = 3, Name = "Leadership Workshop",  Date = new DateTime(2026, 6, 20), Location = "Austin, TX",   Description = "Workshop for emerging leaders.", Capacity = 50,  AttendeesCount = 50 },
            new Event { Id = 4, Name = "Product Launch Gala",  Date = new DateTime(2026, 7, 5),  Location = "San Francisco, CA", Description = "Exclusive product launch event.", Capacity = 200, AttendeesCount = 60 },
            new Event { Id = 5, Name = "Design Sprint",        Date = new DateTime(2026, 8, 12), Location = "Seattle, WA",  Description = "Intensive design thinking sprint.", Capacity = 40,  AttendeesCount = 15 },
        };

        private readonly List<Registration> _registrations = new();

        // ── Event queries ────────────────────────────────────────────────────
        public Task<List<Event>> GetEventsAsync() =>
            Task.FromResult(_events.OrderBy(e => e.Date).ToList());

        public Task<Event?> GetEventByIdAsync(int id) =>
            Task.FromResult(_events.FirstOrDefault(e => e.Id == id));

        public Task<List<Event>> SearchEventsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return GetEventsAsync();

            var lower = query.ToLowerInvariant();
            var results = _events
                .Where(e => e.Name.Contains(lower, StringComparison.OrdinalIgnoreCase)
                         || e.Location.Contains(lower, StringComparison.OrdinalIgnoreCase)
                         || e.Description.Contains(lower, StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.Date)
                .ToList();

            return Task.FromResult(results);
        }

        // ── Registration ─────────────────────────────────────────────────────
        public Task<bool> RegisterForEventAsync(Registration registration)
        {
            var ev = _events.FirstOrDefault(e => e.Id == registration.EventId);
            if (ev is null || !ev.IsAvailable) return Task.FromResult(false);

            registration.Id = _registrations.Count + 1;
            registration.EventName = ev.Name;
            registration.RegisteredAt = DateTime.UtcNow;

            _registrations.Add(registration);
            ev.AttendeesCount++;

            return Task.FromResult(true);
        }

        public Task<List<Registration>> GetRegistrationsAsync() =>
            Task.FromResult(_registrations.ToList());

        public Task<bool> IsAlreadyRegisteredAsync(string email, int eventId) =>
            Task.FromResult(_registrations.Any(r =>
                r.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                r.EventId == eventId));
    }
}