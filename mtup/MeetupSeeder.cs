using Microsoft.EntityFrameworkCore.Internal;
using mtup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mtup
{
    public class MeetupSeeder
    {
        private readonly MeetupContext _meetupContext;

        public MeetupSeeder(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public void Seed()
        {
            if (_meetupContext.Database.CanConnect())
            {
                if (!_meetupContext.Meetups.Any())
                {
                    InsertSampleData();
                }
            }
        }

        private void InsertSampleData()
        {

            var meetups = new List<Meetup>
            {
                new Meetup
                {
                    Name = "M1",
                    Date = DateTime.Now.AddDays(15),
                    IsPrivate = false,
                    Organizer = "O1",
                    Location = new Location
                    {
                        City = "Prze",
                        Street = "Long",
                        PostalCode = "00-111"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "A1.1",
                            Topic = "T1.1",
                            Description = "Desc1.1"
                        },
                        new Lecture
                        {
                            Author = "A1.2",
                            Topic = "T1.2",
                            Description = "Desc1.2"
                        }
                    }
                },
                new Meetup
                {
                    Name = "M2",
                    Date = DateTime.Now.AddDays(35),
                    IsPrivate = true,
                    Organizer = "O2",
                    Location = new Location
                    {
                        City = "Strze",
                        Street = "Short",
                        PostalCode = "11-000"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "A2.1",
                            Topic = "T2.1",
                            Description = "Desc2.1"
                        },
                        new Lecture
                        {
                            Author = "A2.2",
                            Topic = "T2.2",
                            Description = "Desc2.2"
                        }
                    }
                }
            };   
            
            _meetupContext.AddRange(meetups);
            _meetupContext.SaveChanges();
        }
    }
}
