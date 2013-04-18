﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSkeletalAnimation;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.cono;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.target;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.character.characterRepresentation;
using AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.terrain;

namespace AlumnoEjemplos.ValePorUnNombreGeek.src.commandos.character
{
   
    class Enemy : Character
    {
        private ConoDeVision vision;

     
        private float alturaCabeza;
        private const float DEFAULT_VISION_RADIUS = 400;
        private const float DEFAULT_VISION_ANGLE = 60;
        public float VisionAngle { get { return vision.Angle; } set { vision.Angle = value; } }
        public float VisionRadius { get { return vision.Radius; } set { vision.Radius = value; } }
        public bool ConeEnabled { get { return vision.Enabled; } set { vision.Enabled = value; } }

        public Enemy(Vector3 _position, Terrain _terrain)
            : base(_position, _terrain)
        {
            inicializar();
            
        }

  

        public Enemy(Vector3 _position)
            : base(_position)
        {

              inicializar();


        }

        private void inicializar()
        {

            crearConoDeVision(DEFAULT_VISION_RADIUS, DEFAULT_VISION_ANGLE);
        }

        protected override void loadCharacterRepresentation(Vector3 position)
        {
            this.representation = new EnemyRepresentation(position);
        }


        private void crearConoDeVision(float radius, float angle )
        {
            vision = new ConoDeVision(this.representation, radius, angle);
        }

       

        public bool canSee(TgcBox target)
        {
            
            return vision.isInsideVisionRange(target);
        }


        protected bool watch()
        {
            //Si ve a un character lo setea como objetivo y devuelve true
            return false;
        }


        public override void render(float elapsedTime) 
        {
            base.render(elapsedTime);
            vision.renderWireframe();
        }

       

        }



}