﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Animle.Migrations
{
    [DbContext(typeof(AnimleDbContext))]
    [Migration("20240927075442_relationbetweenuser")]
    partial class relationbetweenuser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AnimeWithEmojiDailyChallenge", b =>
                {
                    b.Property<int>("AnimesId")
                        .HasColumnType("int");

                    b.Property<int>("DailyChallengesId")
                        .HasColumnType("int");

                    b.HasKey("AnimesId", "DailyChallengesId");

                    b.HasIndex("DailyChallengesId");

                    b.ToTable("AnimeDailyChallenges", (string)null);
                });

            modelBuilder.Entity("AnimeWithEmojiQuiz", b =>
                {
                    b.Property<int>("AnimesId")
                        .HasColumnType("int");

                    b.Property<int>("QuizzesId")
                        .HasColumnType("int");

                    b.HasKey("AnimesId", "QuizzesId");

                    b.HasIndex("QuizzesId");

                    b.ToTable("AnimeWithEmojiQuiz");
                });

            modelBuilder.Entity("AnimeWithEmojiThreebythree", b =>
                {
                    b.Property<int>("AnimesId")
                        .HasColumnType("int");

                    b.Property<int>("ThreebythreeId")
                        .HasColumnType("int");

                    b.HasKey("AnimesId", "ThreebythreeId");

                    b.HasIndex("ThreebythreeId");

                    b.ToTable("AnimeThreebythree", (string)null);
                });

            modelBuilder.Entity("Animle.Models.AnimeWithEmoji", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EmojiDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("JapaneseTitle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("MyanimeListId")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("properties")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AnimeWithEmoji");
                });

            modelBuilder.Entity("Animle.Models.DailyChallenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DailyChallenges");
                });

            modelBuilder.Entity("Animle.Models.GameContest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DailyChallengeId")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimePlayed")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DailyChallengeId");

                    b.HasIndex("UserId");

                    b.ToTable("GameContests");
                });

            modelBuilder.Entity("Animle.Models.GuessGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnimeWithEmojiId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("AnimeWithEmojiId")
                        .IsUnique();

                    b.ToTable("GuessGames");
                });

            modelBuilder.Entity("Animle.Models.QuizLikes", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("quizId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("UserId", "quizId");

                    b.HasIndex("quizId");

                    b.ToTable("QuizLikes");
                });

            modelBuilder.Entity("Animle.Models.Threebythree", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Threebythree");
                });

            modelBuilder.Entity("Animle.Models.ThreebythreeLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ThreebythreeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThreebythreeId");

                    b.HasIndex("UserId");

                    b.ToTable("threebythreeLikes");
                });

            modelBuilder.Entity("Animle.Models.UnathenticatedGames", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Fingerpring")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("UnathenticatedGames");
                });

            modelBuilder.Entity("Animle.Models.UserGuessGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("GuessGameId")
                        .HasColumnType("int");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GuessGameId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGuessGames");
                });

            modelBuilder.Entity("Animle.Models.Versus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("gameWon")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Versus");
                });

            modelBuilder.Entity("Animle.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Quizes");
                });

            modelBuilder.Entity("Animle.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name", "Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AnimeWithEmojiDailyChallenge", b =>
                {
                    b.HasOne("Animle.Models.AnimeWithEmoji", null)
                        .WithMany()
                        .HasForeignKey("AnimesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.Models.DailyChallenge", null)
                        .WithMany()
                        .HasForeignKey("DailyChallengesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnimeWithEmojiQuiz", b =>
                {
                    b.HasOne("Animle.Models.AnimeWithEmoji", null)
                        .WithMany()
                        .HasForeignKey("AnimesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.Quiz", null)
                        .WithMany()
                        .HasForeignKey("QuizzesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnimeWithEmojiThreebythree", b =>
                {
                    b.HasOne("Animle.Models.AnimeWithEmoji", null)
                        .WithMany()
                        .HasForeignKey("AnimesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.Models.Threebythree", null)
                        .WithMany()
                        .HasForeignKey("ThreebythreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Animle.Models.GameContest", b =>
                {
                    b.HasOne("Animle.Models.DailyChallenge", "Challenge")
                        .WithMany("GameContests")
                        .HasForeignKey("DailyChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.User", "User")
                        .WithMany("GameContests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Challenge");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Animle.Models.GuessGame", b =>
                {
                    b.HasOne("Animle.Models.AnimeWithEmoji", "Anime")
                        .WithOne("GuessGame")
                        .HasForeignKey("Animle.Models.GuessGame", "AnimeWithEmojiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Anime");
                });

            modelBuilder.Entity("Animle.Models.QuizLikes", b =>
                {
                    b.HasOne("Animle.User", "User")
                        .WithMany("QuizLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.Quiz", "LikedQuiz")
                        .WithMany("Likes")
                        .HasForeignKey("quizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LikedQuiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Animle.Models.Threebythree", b =>
                {
                    b.HasOne("Animle.User", "User")
                        .WithOne("Threebythree")
                        .HasForeignKey("Animle.Models.Threebythree", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Animle.Models.ThreebythreeLike", b =>
                {
                    b.HasOne("Animle.Models.Threebythree", "LikedThreebyThree")
                        .WithMany("ThreebythreeLikes")
                        .HasForeignKey("ThreebythreeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.User", "User")
                        .WithMany("ThreebythreeLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LikedThreebyThree");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Animle.Models.UserGuessGame", b =>
                {
                    b.HasOne("Animle.Models.GuessGame", "GuessGame")
                        .WithMany("UserGuessGames")
                        .HasForeignKey("GuessGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animle.User", "user")
                        .WithMany("UserGuessGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GuessGame");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Animle.Models.Versus", b =>
                {
                    b.HasOne("Animle.User", "User")
                        .WithMany("VersusRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Animle.Quiz", b =>
                {
                    b.HasOne("Animle.User", "user")
                        .WithMany("Quizzes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Animle.Models.AnimeWithEmoji", b =>
                {
                    b.Navigation("GuessGame");
                });

            modelBuilder.Entity("Animle.Models.DailyChallenge", b =>
                {
                    b.Navigation("GameContests");
                });

            modelBuilder.Entity("Animle.Models.GuessGame", b =>
                {
                    b.Navigation("UserGuessGames");
                });

            modelBuilder.Entity("Animle.Models.Threebythree", b =>
                {
                    b.Navigation("ThreebythreeLikes");
                });

            modelBuilder.Entity("Animle.Quiz", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Animle.User", b =>
                {
                    b.Navigation("GameContests");

                    b.Navigation("QuizLikes");

                    b.Navigation("Quizzes");

                    b.Navigation("Threebythree");

                    b.Navigation("ThreebythreeLikes");

                    b.Navigation("UserGuessGames");

                    b.Navigation("VersusRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
