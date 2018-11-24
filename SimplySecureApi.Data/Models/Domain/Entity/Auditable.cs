using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public abstract class Auditable : IEquatable<Auditable>
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("dateCreated")]
        [ScaffoldColumn(false)]
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public string DisplayDate => DateCreated.ToString("MMM dd, yyyy");

        public bool Equals(Auditable other)
        {
            return Id == other.Id;
        }
    }
}