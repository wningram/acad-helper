using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RelationshipTrackerLib {
    public class Contact {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string CourseIdsData { get; set; }
        /// <summary>
        /// A list of all the courses this contact has had with the student.
        /// </summary>
        [NotMapped]
        public List<int> CourseIds { get; set; }
        public string TagsData { get; set; }
        [NotMapped]
        public List<string> Tags { get; set; }
        public int Id { get; set; }
        public int OwnerId { get; set; }

        public void DeserializeColumnData() {
            CourseIds = JsonSerializer.Deserialize<List<int>>(CourseIdsData);
            Tags = JsonSerializer.Deserialize<List<string>>(TagsData);
        }
    }
}
