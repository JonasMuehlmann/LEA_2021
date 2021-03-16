using System;
using System.Collections.Generic;
using System.Drawing;


namespace LEA_2021
{
    class Scene
    {
        #region Properties

        public Metadata Metadata { get; set; }

        public List<Object> Objects { get; set; }

        public Bitmap Image { get; set; }

        #endregion

        #region Constructors

        public Scene(Metadata metadata)
        {
            Metadata = metadata;
            Objects  = new List<Object>();
            Image    = new Bitmap(metadata.Width, metadata.Height);
        }

        #endregion


        public void Render()
        {
            throw new NotImplementedException();
        }
    }
}