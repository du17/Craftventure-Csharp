﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorDeEstoque.DAO
{
    class RendaVO
    {

        private Int64 _itemid;
        private String tamnho;
        private Double metragem;
        private DAO dao;
        private conexaoUso conn;

        public RendaVO()
        {

        }

        public Int64 itemid
        {
            get { return _itemid; }
            set { _itemid = value; }
        }
        public String Tamanho
        {
            get { return tamnho; }
            set { tamnho = value; }
        }
        public Double Metragem
        {
            get { return metragem; }
            set { metragem = value; }
        }

        public void Inserir()
        {
            dao = new DAO();
            dao.IDR(Tamanho, Metragem);
        }

        public void Atualizar()
        {
            dao = new DAO();
            dao.ADR(Tamanho, Metragem, itemid);
        }
        public void Remover()
        {
            dao = new DAO();
            dao.RDR(Tamanho, Metragem, itemid);
        }

    }
}