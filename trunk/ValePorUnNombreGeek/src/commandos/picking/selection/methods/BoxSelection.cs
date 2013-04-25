﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX;
using System.Drawing;
using System.Collections;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.character;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.terrain;

namespace AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.picking.selection.multiple
{
    class BoxSelection : SelectionMethod
    {
        private List<Character> selectableCharacters;
        private Terrain terrain;
        private TgcBox selectionBox;
        private Vector3 initTerrainPoint;

        private const float SELECTION_BOX_HEIGHT = 75;


        public BoxSelection(Terrain _terrain, List<Character> _selectableCharacters)
        {
            this.selectableCharacters = _selectableCharacters;
            this.terrain = _terrain;

            this.selectionBox = TgcBox.fromSize(new Vector3(3, SELECTION_BOX_HEIGHT, 3), Color.Red);
            this.selectionBox.AlphaBlendEnable = true;
            this.selectionBox.Color = Color.FromArgb(110, Color.CadetBlue);
        }

        /*private void selectCharactersByRay(TgcRay _ray)
        {
            foreach (Character ch in this.selectableCharacters)
            {
                Vector3 collisionPoint; //useless
                if (TgcCollisionUtils.intersectRayAABB(_ray, ch.BoundingBox(), out collisionPoint))
                {
                    this.addSelectedCharacter(ch);
                    break;
                }
            }
        }*/

        private List<Character> getCharactersInBox(TgcBox _selectionBox)
        {
            List<Character> ret = new List<Character>();

            foreach (Character ch in this.selectableCharacters)
                if (TgcCollisionUtils.testAABBAABB(_selectionBox.BoundingBox, ch.BoundingBox()))
                    ret.Add(ch);

            return ret;
        }

        public bool canBeginSelection()
        {
            return PickingRaySingleton.Instance.terrainIntersection(this.terrain, out this.initTerrainPoint);
        }

        public void renderSelection()
        {
            Vector3 terrainPointB;
            if (!PickingRaySingleton.Instance.terrainIntersection(this.terrain, out terrainPointB)) return;
            Vector3 terrainPointA = this.initTerrainPoint;

            float selectionBoxHeight = FastMath.Max(terrainPointA.Y, terrainPointB.Y) + SELECTION_BOX_HEIGHT;

            terrainPointA.Y = 0;
            terrainPointB.Y = 0;

            Vector3 min = Vector3.Minimize(terrainPointA, terrainPointB);
            Vector3 max = Vector3.Maximize(terrainPointA, terrainPointB);
            min.Y = 0;
            max.Y = selectionBoxHeight;

            this.selectionBox.setExtremes(min, max);
            this.selectionBox.updateValues();
            this.selectionBox.render();
        }

        public List<Character> endAndRetSelection()
        {
            return this.getCharactersInBox(this.selectionBox);
        }
    }
}
