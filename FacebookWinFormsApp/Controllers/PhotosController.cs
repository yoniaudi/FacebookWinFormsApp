﻿using FacebookWrapper.ObjectModel;
using System;
using System.Windows.Forms;

namespace BasicFacebookFeatures.Models
{
    public partial class PhotosController : UserControl, IControllers
    {
        public string DisplayMember { get { return "Name"; } }
        public object DataSource { get; set; }
        private ProgressBar m_ProgressBar = null;

        public PhotosController()
        {
            InitializeComponent();
        }

        public PhotosController(FacebookObjectCollection<Album> i_Albums, ProgressBar i_ProgressBar)
        {
            InitializeComponent();

            try
            {
                m_ProgressBar = i_ProgressBar;
                initializeProgressBar(i_Albums);
                DataSource = filterAlbumsWithProgress(i_Albums);
            }
            catch(Exception ex)
            {
                string exMsg = string.Format("Getting albums is not supported by Meta anymore.{0}Press ok to continue.{0}Error: {1}",
                    Environment.NewLine, ex.Message);

                MessageBox.Show(exMsg);
            }
        }

        private void initializeProgressBar(FacebookObjectCollection<Album> i_Albums = null, FacebookWrapper.ObjectModel.Album i_Album = null)
        {
            m_ProgressBar.Invoke(new Action(() =>
            {
                m_ProgressBar.Visible = true;
                m_ProgressBar.Minimum = 0;
                m_ProgressBar.Maximum = i_Albums != null ? i_Albums.Count : (i_Album != null ? i_Album.Photos.Count : 0);
                m_ProgressBar.Value = 0;
                m_ProgressBar.Step = 1;
                m_ProgressBar.Visible = false;
            }));
        }

        private object filterAlbumsWithProgress(FacebookObjectCollection<Album> i_Albums)
        {
            FacebookObjectCollection<Album> filteredAlbums = new FacebookObjectCollection<Album>();

            foreach (FacebookWrapper.ObjectModel.Album album in i_Albums)
            {
                if (album.Count > 0)
                {
                    filteredAlbums.Add(album);
                }

                m_ProgressBar.Invoke(new Action(() => m_ProgressBar.PerformStep()));
            }

            return filteredAlbums;
        }

        /*public void ShowSelectedAlbum(FacebookWrapper.ObjectModel.Album i_Album)
        {
            initializeProgressBar(null, i_Album);
            flowLayoutPanelPhotos.Controls.Clear();

            for (int i = 0; i < i_Album.Photos.Count; i++)
            {
                PictureBox picture = new PictureBox();

                picture.Name = $"pictureBox{i}";
                picture.SizeMode = PictureBoxSizeMode.AutoSize;
                picture.ImageLocation = i_Album.Photos[i].PictureThumbURL;
                flowLayoutPanelPhotos.Controls.Add(picture);
                picture.BringToFront();
                picture.Visible = true;
                m_ProgressBar.Invoke(new Action(() => m_ProgressBar.PerformStep()));
            }
        }*/

        public void Show(object i_Object)
        {
            Album album = i_Object as Album;

            initializeProgressBar(null, album);
            flowLayoutPanelPhotos.Controls.Clear();

            for (int i = 0; i < album.Photos.Count; i++)
            {
                PictureBox picture = new PictureBox();

                picture.Name = $"pictureBox{i}";
                picture.SizeMode = PictureBoxSizeMode.AutoSize;
                picture.ImageLocation = album.Photos[i].PictureThumbURL;
                flowLayoutPanelPhotos.Controls.Add(picture);
                picture.BringToFront();
                picture.Visible = true;
                m_ProgressBar.Invoke(new Action(() => m_ProgressBar.PerformStep()));
            }
        }
    }
}
