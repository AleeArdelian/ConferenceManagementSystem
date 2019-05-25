--scriptul acesta genereaza baza de date--
--am schimbat coloanele CNP din bigint in varchar(14)--
--rulati scriptul apoi modificat in 'App.config' connection string-ul pentru a putea folosi baza de date :)--

CREATE DATABASE [ISS-DB]

USE [ISS-DB]

CREATE TABLE SteeringCommittee(
	Username varchar(20),
	Passwd varchar(30),
	PRIMARY KEY(Username, Passwd)
)

CREATE TABLE Abstracts(
	AbstractID int PRIMARY KEY IDENTITY(1,1),
	Content varchar(100),
	AbstractName varchar(20)
)

CREATE TABLE Sections(
	SectionID int PRIMARY KEY IDENTITY(1,1),
	SectionChairCNP varchar(14),
	Room varchar(10)
)

CREATE TABLE Papers(
	PaperID int PRIMARY KEY IDENTITY(1,1),
	SectionID int REFERENCES Sections(SectionID),
	Content varchar(max),
	PaperName varchar(20),
	Topic varchar(20)
)

CREATE TABLE Participants(
	CNP varchar(14) PRIMARY KEY,
	FirstName varchar(15),
	LastName varchar(15),
	Email varchar(30),
	Username varchar(20),
	Passwd varchar(30)
)

CREATE TABLE ParticipantSections(
	SectionID int REFERENCES Sections(SectionID),
	ParticipantCNP varchar(14) REFERENCES Participants(CNP),
	PRIMARY KEY(SectionID, ParticipantCNP)
)

CREATE TABLE Authors(
	CNP varchar(14) PRIMARY KEY,
	FirstName varchar(15),
	LastName varchar(15),
	Email varchar(30),
	Username varchar(20),
	Passwd varchar(30),
	Affiliation varchar(40)
)

CREATE TABLE Proposal(
	AuthorCNP varchar(14) REFERENCES Authors(CNP),
	ProposalName varchar(20),
	AbstractID int REFERENCES Abstracts(AbstractID),
	PaperID int REFERENCES Papers(PaperID),
	PRIMARY KEY(AuthorCNP, ProposalName)
)

CREATE TABLE Conferences(
	ConferenceID int PRIMARY KEY IDENTITY(1,1),
	ConferenceName varchar(20)
)

CREATE TABLE ConferenceAuthors(
	ConferenceID int REFERENCES Conferences(ConferenceID),
	AuthorCNP varchar(14) REFERENCES Authors(CNP),
	PRIMARY KEY(ConferenceID, AuthorCNP)
)

CREATE TABLE ChosenProgramCommittee(
	CNP varchar(14) PRIMARY KEY,
	PCRole varchar(10)
)

CREATE TABLE ConferenceChosenProgramCommittee(
	ConferenceID int REFERENCES Conferences(ConferenceID),
	PCCNP varchar(14) REFERENCES ChosenProgramCommittee(CNP)
	PRIMARY KEY(ConferenceID, PCCNP)
)

CREATE TABLE ChairMembers(
	CNP varchar(14) PRIMARY KEY,
	FirstName varchar(15),
	LastName varchar(15),
	Email varchar(30),
	Website varchar(30),
	Username varchar(20),
	Passwd varchar(30),
	Affiliation varchar(40)
)

CREATE TABLE ConferenceChairMembers(
	ConferenceID int REFERENCES Conferences(ConferenceID),
	ChairMemberCNP varchar(14) REFERENCES ChairMembers(CNP)
	PRIMARY KEY(ConferenceID, ChairMemberCNP)
)

CREATE TABLE AuthorPapers(
	AuthorCNP varchar(14) REFERENCES Authors(CNP),
	PaperID int REFERENCES Papers(PaperID),
	PRIMARY KEY(AuthorCNP, PaperID)
)

CREATE TABLE SpeakersSections(
	SectionID int REFERENCES Sections(SectionID),
	AuthorCNP varchar(14) REFERENCES Authors(CNP),
	PRIMARY KEY(SectionID, AuthorCNP)
)

CREATE TABLE ConferenceParticipants(
	ConferenceID int REFERENCES Conferences(ConferenceID),
	ParticipantCNP varchar(14) REFERENCES Participants(CNP),
	PRIMARY KEY(ConferenceID, ParticipantCNP)
)

CREATE TABLE RegularMembers(
	CNP varchar(14) PRIMARY KEY,
	FirstName varchar(15),
	LastName varchar(15),
	Email varchar(30),
	Website varchar(30),
	Username varchar(20),
	Passwd varchar(30),
	Affiliation varchar(40),
	isReviewer bit
)

CREATE TABLE Reviews(
	PaperID int REFERENCES Papers(PaperID),
	ReviewerCNP varchar(14) REFERENCES RegularMembers(CNP),
	Qualifier varchar(15),
	Comments varchar(50),
	ReevalRequest bit
	PRIMARY KEY(PaperID, ReviewerCNP)
)

CREATE TABLE ReevalRequests(
	RequestID int PRIMARY KEY IDENTITY(1,1),
	PaperID int REFERENCES Papers(PaperID),
	ReviewerCNP varchar(14) REFERENCES RegularMembers(CNP)
)