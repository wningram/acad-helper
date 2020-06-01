using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RelationshipTrackerLib {
    /// <summary>
    /// This class is intended to be an ASP .Net service.
    /// </summary>
    class Contacts {
        private DbContext _dataContext;

        public Contacts(DbContext dataContext) {
            _dataContext = dataContext;
        }

        public List<Contact> AllContacts { get; protected set; }

        public Contact GetContact(int id) {
            DbSet<Contact> contacts = _dataContext.Set<Contact>();
            return contacts.First(c => c.Id == id);
        }

        public void UpdateContact(Contact contact) {
            DbSet<Contact> contacts = _dataContext.Set<Contact>();
            contacts.Update(contact);
        }
        
        public void CreateContact(Contact contact) {
            DbSet<Contact> contacts = _dataContext.Set<Contact>();
            contacts.Add(contact);
        }

        public void RemoveContact(int id) {
            DbSet<Contact> contacts = _dataContext.Set<Contact>();
            contacts.Remove(
                    contacts.Find(id));
        }
    }
}
