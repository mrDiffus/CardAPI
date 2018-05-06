using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CardAPI.Model
{
    public class CardContext : DbContext
    {
        public DbSet<Card> Cards {get;set;}
        public DbSet<CardOption> CardOptions {get;set;}
        public DbSet<Game> Games {get;set;}
        public DbSet<CardState> CardStates {get;set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Cards.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Card>(x => 
            {
                x.HasKey(y => y.CardID);
                x.HasMany(y => y.Options).WithOne(o => o.Card).IsRequired(true);
                x.HasMany(y => y.StagedBy).WithOne(o => o.toBeStaged).IsRequired(false);
            }
            );
            modelBuilder.Entity<CardOption>(x =>
            {
                x.HasKey( y => y.CardOptionID);
                x.HasOne( y => y.Card).WithMany(c => c.Options ).IsRequired(true);
                x.HasMany(c => c.Stages).WithOne(s => s.StagedBy);
            } 
            );
            modelBuilder.Entity<Stage>().HasKey(x => new {x.CardID,x.CardOptionID});

            modelBuilder.Entity<CardState>().HasKey(x => new {x.CardID, x.GameID});
            modelBuilder.Entity<CardState>().HasOne(x => x.Game).WithMany(y => y.CardStates).IsRequired(true);
            modelBuilder.Entity<CardState>().HasOne(x => x.Card).WithMany().IsRequired(true);

            modelBuilder.Entity<Game>().HasKey(x => x.GameID);
            modelBuilder.Entity<Game>().HasMany(x => x.CardStates).WithOne(y => y.Game).IsRequired(true);

    }
    }
    public class Card 
    {
        public int CardID {get;set;}
        public string CardName{get;set;}
        public int CardNumber{get;set;}
        public CardType CardType{get;set;}
        public List<CardOption> Options{get;set;}

        public List<Stage> StagedBy{get;set;}
    }

    public class CardOption {
        public int CardOptionID{get;set;}
        [JsonIgnore]
        public Card Card{get;set;}
        public string Action{get;set;}
        public string Result{get;set;}
        public List<Stage> Stages {get;set;}
    }

    public class Stage {
        public int CardOptionID {get;set;}
        public int CardID {get;set;}
        public Card toBeStaged {get;set;}
        public CardOption StagedBy {get;set;}
    }

    public class Game 
    {
        public int GameID {get;set;}
        public List<CardState> CardStates {get;set;}
    }

    public class CardState 
    {
        public int CardID {get;set;}
        public Card Card {get;set;}
        public int GameID {get;set;}
        [JsonIgnore]
        public Game Game {get;set;}
    } 

    public enum CardType {
        Ruins,
        Settlement,
        Quest
    }

}