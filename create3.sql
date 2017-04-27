copy business(bid,name,city,state,latitude,longitude,stars,num_checkins,full_addr,zip,review_count)
FROM 'C:\Users\mvs6_000\Desktop\Databases\business.csv' DELIMITER ',' CSV HEADER;

copy user_table(uid,name,review_count,cool,funny,useful,average_stars,vote_count,time_yelping,fans)
FROM 'C:\Users\mvs6_000\Desktop\Databases\user_table.csv' DELIMITER ',' CSV HEADER;


create table Business(
	bid VARCHAR(24) NOT NULL,
	name VARCHAR(100) NOT NULL,
	city VARCHAR(40),
	state VARCHAR(4),
    latitude DOUBLE PRECISION,
	longitude DOUBLE PRECISION,
	stars dec,
	num_checkins INT,
    full_addr VARCHAR(200),
    zip numeric,
    review_count numeric,
	PRIMARY KEY (bid)
);

create table Categories(
  	bid varchar(24),
    category varchar(100)
);

create table Hours(
    bid varchar(24) not null,
    monOpen varchar(10),
    monClose varchar(10),
    tueOpen varchar(10),
    tueClose varchar(10),
    wedOpen varchar(10),
    wedClose varchar(10),
    thurOpen varchar(10),
    thurClose varchar(10),
    friOpen varchar(10),
    friClose varchar(10),
    satOpen varchar(10),
    satClose varchar(10),
    sunOpen varchar(10),
    sunClose varchar(10),
    primary key(bid),
    foreign key (bid) references Business(bid)
);

create table User_table(
    uid VARCHAR(22) NOT NULL,
	name VARCHAR(100) NOT NULL,	
    review_count numeric,
    cool numeric,
    funny numeric,
    useful numeric,
    average_stars dec,
    vote_count numeric,
	time_yelping VARCHAR(20),
	fans INT,
	PRIMARY KEY (uid)
);

create table Friends(
    uid varchar(24),
    friend_uid varchar (24)
);

create table Tips(
    uid varchar(24) not null,
    bid varchar(24) not null,
    tip varchar(1000),
    date_of_tip date not null,
    likes numeric
);

create table Check_In(
    bid varchar(24) not null,
    mon numeric,
    tue numeric,
    wed numeric,
    thur numeric,
    fri numeric,
    sat numeric,
    sun numeric,
    PRIMARY KEY(bid),
	FOREIGN KEY(bid) REFERENCES Business(bid)
);