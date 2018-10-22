using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity.Test
{
	/// <summary>
	///     Represents a Role entity
	/// </summary>
	public class PocoRole : PocoRole<string>
	{
		/// <summary>
		///     Constructor
		/// </summary>
		public PocoRole()
		{
			Id = Guid.NewGuid().ToString();
		}

		/// <summary>
		///     Constructor
		/// </summary>
		/// <param name="roleName"></param>
		public PocoRole(string roleName) : this()
		{
			Name = roleName;
		}
	}

	/// <summary>
	///     Represents a Role entity
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class PocoRole<TKey> where TKey : IEquatable<TKey>
	{
		/// <summary>
		///     Constructor
		/// </summary>
		public PocoRole() { }

		/// <summary>
		///     Constructor
		/// </summary>
		/// <param name="roleName"></param>
		public PocoRole(string roleName) : this()
		{
			Name = roleName;
		}

		/// <summary>
		///     Role id
		/// </summary>
		public virtual TKey Id { get; set; }

		/// <summary>
		/// Navigation property for claims in the role
		/// </summary>
		public virtual ICollection<PocoRoleClaim<TKey>> Claims { get; private set; } = new List<PocoRoleClaim<TKey>>();

		/// <summary>
		///     Role name
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Normalized name used for equality
		/// </summary>
		public virtual string NormalizedName { get; set; }

		/// <summary>
		/// A random value that should change whenever a role is persisted to the store
		/// </summary>
		public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
	}

	/// <summary>
	///     EntityType that represents one specific role claim
	/// </summary>
	public class PocoRoleClaim : PocoRoleClaim<string> { }

	/// <summary>
	///     EntityType that represents one specific role claim
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class PocoRoleClaim<TKey> where TKey : IEquatable<TKey>
	{
		/// <summary>
		///     Primary key
		/// </summary>
		public virtual int Id { get; set; }

		/// <summary>
		///     User Id for the role this claim belongs to
		/// </summary>
		public virtual TKey RoleId { get; set; }

		/// <summary>
		///     Claim type
		/// </summary>
		public virtual string ClaimType { get; set; }

		/// <summary>
		///     Claim value
		/// </summary>
		public virtual string ClaimValue { get; set; }
	}
}
