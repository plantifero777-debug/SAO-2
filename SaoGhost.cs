using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace SaoPlugin
{
    public partial class SaoPlugin
    {
        // ================= Recoger objetos (fantasma) =================
        // Cada fantasma recuerda lo que llevaba al morir. Si su inventario gana objetos, se devuelven al suelo.
        readonly Dictionary<int, Dictionary<int, int>> ghostInventory = new Dictionary<int, Dictionary<int, int>>();

        static Dictionary<int, int> CountInventory(TSPlayer p)
        {
            var counts = new Dictionary<int, int>();
            foreach (var item in p.TPlayer.inventory)
            {
                if (item == null || item.type == 0 || item.stack <= 0) continue;
                counts[item.type] = counts.TryGetValue(item.type, out int c) ? c + item.stack : item.stack;
            }
            return counts;
        }

        void CheckGhostPickups()
        {
            foreach (var p in TShock.Players)
            {
                if (p == null || !p.Active) continue;
                var d = GetData(p);
