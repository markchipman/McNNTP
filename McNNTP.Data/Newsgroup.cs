﻿using System;
using JetBrains.Annotations;
using System.Collections.Generic;
using NHibernate;

namespace McNNTP.Data
{
    public class Newsgroup
    {
        public virtual int Id { get; set; }
        [NotNull]
        public virtual string Name { get; set; }
        [CanBeNull]
        public virtual string Description { get; set; }
        public virtual bool Moderated { get; set; }
        public virtual int PostCount { get; set; }
        public virtual int? LowWatermark { get; set; }
        public virtual int? HighWatermark { get; set; }
        public virtual DateTime CreateDate { get; set; }
        [CanBeNull]
        public virtual string CreatorEntity { get; set; }

        public virtual IList<Newsgroup> ModeratedBy { get; set; }


        [NotNull, Pure]
        public virtual Newsgroup GetMetaCancelledGroup([NotNull] ISession session)
        {
            var counts = (object[])session.CreateQuery(
                    "select min(an.Number), max(an.Number), count(an.Id) from ArticleNewsgroup an where an.Cancelled = 1 and an.Newsgroup.Name = :NewsgroupName")
                    .SetParameter("NewsgroupName", Name)
                    .UniqueResult();

            return new Newsgroup
            {
                CreateDate = CreateDate,
                CreatorEntity = CreatorEntity,
                Description = "Cancelled posts for " + Name,
                HighWatermark = counts[1] == null ? 0 : (int)counts[1],
                Id = 0,
                LowWatermark = counts[0] == null ? 0 : (int)counts[0],
                PostCount = Convert.ToInt32(counts[2]),
                Name = Name + ".deleted",
                Moderated = true
            };
        }

        [NotNull, Pure]
        public virtual Newsgroup GetMetaPendinGroup([NotNull] ISession session)
        {
            var counts = (object[])session.CreateQuery(
                   "select min(an.Number), max(an.Number), count(an.Id) from ArticleNewsgroup an where an.Pending = 1 and an.Newsgroup.Name = :NewsgroupName")
                   .SetParameter("NewsgroupName", Name)
                   .UniqueResult();

            return new Newsgroup
            {
                CreateDate = CreateDate,
                CreatorEntity = CreatorEntity,
                Description = "Pending posts for " + Name,
                HighWatermark = counts[1] == null ? 0 : (int)counts[1],
                Id = 0,
                LowWatermark = counts[0] == null ? 0 : (int)counts[0],
                PostCount = Convert.ToInt32(counts[2]),
                Name = Name + ".pending",
                Moderated = true
            };
        }
    }
}