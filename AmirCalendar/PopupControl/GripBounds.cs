// Type: PopupControl.GripBounds
// Assembly: PopupControl, Version=1.4.0.968, Culture=neutral, PublicKeyToken=04c10344d6495b18
// Assembly location: C:\Users\programmer\Desktop\FarsiDate Win App\FarsiDate\FarsiDate\bin\Debug\PopupControl.dll

using System.Drawing;

namespace AmirCalendar.PopupControl
{
    internal struct GripBounds
    {
        private readonly Rectangle _clientRectangle;

        public Rectangle ClientRectangle
        {
            get
            {
                return this._clientRectangle;
            }
        }

        public Rectangle Bottom
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Y = clientRectangle.Bottom - 6 + 1;
                clientRectangle.Height = 6;
                return clientRectangle;
            }
        }

        public Rectangle BottomRight
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Y = clientRectangle.Bottom - 12 + 1;
                clientRectangle.Height = 12;
                clientRectangle.X = clientRectangle.Width - 12 + 1;
                clientRectangle.Width = 12;
                return clientRectangle;
            }
        }

        public Rectangle Top
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Height = 6;
                return clientRectangle;
            }
        }

        public Rectangle TopRight
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Height = 12;
                clientRectangle.X = clientRectangle.Width - 12 + 1;
                clientRectangle.Width = 12;
                return clientRectangle;
            }
        }

        public Rectangle Left
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Width = 6;
                return clientRectangle;
            }
        }

        public Rectangle BottomLeft
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Width = 12;
                clientRectangle.Y = clientRectangle.Height - 12 + 1;
                clientRectangle.Height = 12;
                return clientRectangle;
            }
        }

        public Rectangle Right
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.X = clientRectangle.Right - 6 + 1;
                clientRectangle.Width = 6;
                return clientRectangle;
            }
        }

        public Rectangle TopLeft
        {
            get
            {
                var clientRectangle = this.ClientRectangle;
                clientRectangle.Width = 12;
                clientRectangle.Height = 12;
                return clientRectangle;
            }
        }

        public GripBounds(Rectangle clientRectangle)
        {
            this._clientRectangle = clientRectangle;
        }
    }
}
