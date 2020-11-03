using NPoco;
using SimpleBotWeb.Models.DataObjects;
using SimpleBotWeb.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBotWeb.Models.DataHelpers
{
    public class EntryHelper
    {
        private Database dc;

        public EntryHelper(Database dataContext)
        {
            dc = dataContext;
        }

        #region Entries
        public IList<Entry> GetEntries()
        {
            var sql = "select entries.*, Users.Name as Owner, (select Users.Name from Users join EditLog on Users.MemberId=EditLog.MemberId where EditLog.EntryId=Entries.EntryID order by DateEditedUtc desc limit 1) as LastEdited " +
                      "from entries join Users on (entries.MemberId = Users.MemberId) " +
                      "group by entryid, phrase, response, startswith, hidden";

            return dc.Query<Entry>(sql).ToList();
        }

        public void Add(string phrase = "", string response = "", bool startsWith = false, bool hidden = false, int memberId = 0, bool allowRepeat = false)
        {
            if (string.IsNullOrWhiteSpace(phrase) || string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            if (!UtilityHelper.IsValidRegex(phrase))
            {
                return;
            }

            var entry = GetEntry(phrase, response, startsWith);

            if (entry == null)
            {
                entry = new Entry
                {
                    Hidden = hidden,
                    Phrase = phrase,
                    Response = response,
                    StartsWith = startsWith,
                    MemberId = memberId,
                    AllowRepeat = allowRepeat
                };
                SaveEntry(entry);
            }

            CreateEditLog(entry);
        }

        public void SaveEntry(Entry entity)
        {
            if (entity != null)
            {
                var tu = new TinyUrlHelper(dc);
                entity.Response = tu.ShortenAllUrlsInString(entity.Response);
                dc.Save(entity);
                CreateEditLog(entity);
            }
        }

        public Entry GetEntry(string phrase = "", string response = "", bool startsWith = false)
        {
            var sql = "select * from entries where phrase=@0 and StartsWith=@1 and response=@2";
            return dc.FirstOrDefault<Entry>(sql, phrase.ToLowerInvariant(), startsWith, response);
        }

        public Entry GetEntryById(int entryId)
        {
            return dc.FirstOrDefault<Entry>("select * from entries where EntryId=@0", entryId);
        }

        public void Delete(int entryId)
        {
            dc.Execute("delete from entries where EntryId=@0", entryId);
        }
        #endregion

        #region Logs
        private void CreateEditLog(Entry entity)
        {
            if (entity == null)
                return;

            var el = new EditLog
            {
                EntryId = entity.EntryId,
                MemberId = entity.MemberId,
                Response = entity.Response,
                DateEditedUtc = DateTime.UtcNow,
                Hidden = entity.Hidden,
                Phrase = entity.Phrase,
                StartsWith = entity.StartsWith,
                AllowRepeat = entity.AllowRepeat
            };
            SaveEditLog(el);
        }

        public void SaveEditLog(EditLog entity)
        {
            if (entity != null)
            {
                dc.Save(entity);
            }
        }

        public IList<EditLog> GetEditLogsByEntryId(int entryId)
        {
            return dc.Query<EditLog>("Select * from EditLog where EntryId=@0 order by DateEditedUtc asc", entryId).ToList();
        }
        #endregion
    }
}
