﻿using WatchMovie.Domain.Common;
using WatchMovie.Domain.Entities.Users;
namespace WatchMovie.Domain.Entities.Movies
{
	public class MovieFeedBack : BaseEntity
	{
		public string? Comment { get; set; }
		public int Rate { set; get; }
		public Guid UserId { get; set; }
		public Guid MovieId { get; set; }
		public virtual Movie Movie { get; set; }
		public virtual User Users { get; set; }
	}
}
