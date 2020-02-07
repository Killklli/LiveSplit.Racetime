﻿using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Racetime.Model
{
    public class Race : RTModelBase
    {
        public static dynamic EntrantToUserConverter(dynamic e)
        {
            
            return new
            {
               id = e.user.id,
                name = e.user.name,
                full_name = e.user.full_name,
                flair = e.user.flair,
                twitch_name = e.user.twitch_name,
                twitch_channel = e.user.twitch_channel,
                discriminator = e.user.discriminator,
                status = e.status
            };
        }
        public bool AllowNonEntrantChat
        {
            get
            {
                return false;
            }
        }
        public bool AllowMidraceChat
        {
            get
            {
                return Data.allow_midrace_chat;
            }
        }
        public bool AllowComments
        {
            get
            {
                return Data.allow_comments;
            }
        }
        public string ID
        {
            get
            {
                return Data.name;
            }
        }
        public string ChannelName
        {
            get
            {
                return ID.Substring(0,ID.IndexOf('/'));
            }
        }
        public string Name
        {
            get
            {
                return Data.name;
            }
        }
        public string Goal
        {
            get
            {
                try
                {
                    return Data.goal.name;
                }
                catch
                {
                    return null;
                }
            }
        }
        public string Info
        {
            get
            {
                try
                {
                    return Data.info;
                }
                catch
                {
                    return null;
                }
            }
        }
        public TimeSpan StartDelay
        {
            get
            {
                try
                {
                    return TimeSpan.Parse(Data.start_delay);
                }
                catch
                {
                    return TimeSpan.Zero;
                }
            }
        }
        public int NumEntrants
        {
            get
            {
                return Data.entrants_count;
            }
        }
        public IEnumerable<RacetimeUser> Entrants
        {
            get
            {
                foreach(var e in Data.entrants)
                {
                    yield return RTModelBase.Create<RacetimeUser>(EntrantToUserConverter(e));
                }
            }
        }
        public string GameSlug
        {
            get
            {
                return ID.Substring(ID.IndexOf('/')+1);
            }
        }
        public RaceState State
        {
            get
            {
                switch(Data.status.value)
                {
                    case "open": return RaceState.Open; 
                    default: return RaceState.Unknown;
                }
            }
        }
        public DateTime? StartedAt
        {
            get
            {
                try
                {
                    if (Data.started_at == null)
                        return DateTime.MaxValue;
                    return DateTime.Parse(Data.started_at);
                }
                catch
                {
                    return DateTime.MaxValue;
                }
            }
        }
       
        public int NumFinishes
        {
            get
            {
                return Entrants.Count(x => x.Status == UserStatus.Finished);
            }
        }
        public int NumDropOuts
        {
            get
            {
                return Entrants.Count(x => x.Status == UserStatus.Forfeit || x.Status == UserStatus.Disqualified);
            }
        }
        public DateTime OpenedAt
        {
            get
            {
                try
                {
                    if (Data.opened_at == null)
                        return DateTime.MaxValue;
                    return DateTime.Parse(Data.opened_at);
                }
                catch
                {
                    return DateTime.MaxValue;
                }
            }
        }
        public RacetimeUser OpenedBy
        {
            get
            {
                return RTModelBase.Create<RacetimeUser>(Data.opened_by);
            }
        }
    }
}
