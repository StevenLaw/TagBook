using LiteDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagModel.Model;
using TagModel.ViewModel.Commands;

namespace TagModel.ViewModel
{
    public class TagViewModel : PropertyNotifier
    {
        private const string ENTRY_COLLECTION = "Entry";

        #region Properties
        private ObservableCollection<Entry> entries = new ObservableCollection<Entry>();
        private ObservableCollection<Tag> tags;
        private string filename;

        public ObservableCollection<Entry> Entries
        {
            get => entries;
            set
            {
                entries = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Tag> Tags
        {
            get => tags;
            private set
            {
                tags = value;
                OnPropertyChanged();
            }
        }

        public string Filename
        {
            get => filename;
            set
            {
                filename = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public AddLinkEntryCommand AddLinkEntryCommand { get; set; }
        #endregion

        #region Events
        public event EventHandler<ErrorEncounteredErrorEventArgs> ErrorEncountered;
        public event EventHandler ViewMainList;
        public event EventHandler<AddEditEntryEventArgs> AddEditEntry;
        #endregion

        public TagViewModel()
        {
            InstantiateCommands();
        }

        private void InstantiateCommands()
        {
            AddLinkEntryCommand = new AddLinkEntryCommand(this);
        }

        #region Event Access
        public void AddEntry(Type entryType)
        {
            AddEditEntry?.Invoke(this, new AddEditEntryEventArgs(entryType));
        }
        #endregion

        #region Data Access
        private bool IsFilenameEmpty()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ErrorType.FilenameNotSet));
                return true;
            }
            return false;
        }

        public void LoadFile()
        {
            if (IsFilenameEmpty()) return;
            using (var db = new LiteDatabase(Filename))
            {
                LoadFile(db);
            }
        }

        protected void LoadFile(LiteDatabase db)
        {
            try
            {
                var col = db.GetCollection<Entry>(ENTRY_COLLECTION);
                entries = new ObservableCollection<Entry>(col.Query().ToEnumerable());
            }
            catch (Exception ex)
            {
                ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ex));
            }
        }

        public int? InsertEntry(Entry entry)
        {
            if (IsFilenameEmpty()) return null;
            using (var db = new LiteDatabase(Filename))
            {
                try
                {
                    var col = db.GetCollection<Entry>(ENTRY_COLLECTION);
                    int amount = col.Insert(entry).AsInt32;
                    LoadFile(db);
                    return amount;
                }
                catch (Exception ex)
                {
                    ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ex));
                }
            }
            return null;
        }

        public bool UpdateEntry(Entry entry)
        {
            if (IsFilenameEmpty()) return false;
            using (var db = new LiteDatabase(Filename))
            {
                try
                {
                    var col = db.GetCollection<Entry>(ENTRY_COLLECTION);
                    bool success = col.Update(entry);
                    LoadFile(db);
                    return success;
                }
                catch (Exception ex)
                {
                    ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ex));
                    return false;
                }
            }
        }

        public bool DeleteEntry(int id)
        {
            if (IsFilenameEmpty()) return false;
            using (var db = new LiteDatabase(Filename))
            {
                try
                {
                    var col = db.GetCollection<Entry>(ENTRY_COLLECTION);
                    bool success = col.Delete(new BsonValue(id));
                    LoadFile(db);
                    return success;
                }
                catch (Exception ex)
                {
                    ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ex));
                    return false;
                }
            }
        }

        public void ReloadTags()
        {
            Tags = new ObservableCollection<Tag>(GetTags());
        }

        public HashSet<Tag> GetTags()
        {
            if (IsFilenameEmpty()) return null;
            using (var db = new LiteDatabase(Filename))
            {
                try
                {
                    var col = db.GetCollection<Entry>(ENTRY_COLLECTION);
                    col.EnsureIndex(x => x.Tags);
                    return col.Query().Where(x => x.Tags != null).ToArray().SelectMany(x => x.Tags).ToHashSet();
                }
                catch (Exception ex)
                {
                    ErrorEncountered?.Invoke(this, new ErrorEncounteredErrorEventArgs(ex));
                    return null;
                }
            }
        }
        #endregion

        #region Navigation
        public void GoToMainList()
        {
            ViewMainList?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
