using ElectronSharp.API.Entities;
using System;
using System.Collections.Generic;

namespace ElectronSharp.API.Extensions
{
    internal static class ThumbarButtonExtensions
    {
        public static ThumbarButton[] AddThumbarButtonsId(this ThumbarButton[] thumbarButtons)
        {
            for (int index = 0; index < thumbarButtons.Length; index++)
            {
                var thumbarButton = thumbarButtons[index];

                if (string.IsNullOrEmpty(thumbarButton.Id))
                {
                    thumbarButton.Id = Guid.NewGuid().ToString();
                }
            }

            return thumbarButtons;
        }

        public static ThumbarButton GetThumbarButton(this List<ThumbarButton> thumbarButtons, string id)
        {
            ThumbarButton result = new("");

            foreach (var item in thumbarButtons)
            {
                if (item.Id == id)
                {
                    result = item;
                }
            }

            return result;
        }
    }
}
