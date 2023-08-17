USE master
GO

--drop database if it exists
IF DB_ID('final_capstone') IS NOT NULL
BEGIN
	ALTER DATABASE final_capstone SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE final_capstone;
END

CREATE DATABASE final_capstone
GO

USE final_capstone
GO

DROP TABLE IF EXISTS [menu_items];
DROP TABLE IF EXISTS [menu_order];
DROP TABLE IF EXISTS [cookout_user];
DROP TABLE IF EXISTS [invite_user]
DROP TABLE IF EXISTS [invites];
DROP TABLE IF EXISTS [users];
DROP TABLE IF EXISTS [orders];
DROP TABLE IF EXISTS [cookout];

--create tables
CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username nvarchar(255) NOT NULL,
	name varchar(50) NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL,

	CONSTRAINT PK_user PRIMARY KEY (user_id),
)

--populate default data
INSERT INTO users (username, name, password_hash, salt, user_role) VALUES ('user@gmail.com','user','Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user');
INSERT INTO users (username, name, password_hash, salt, user_role) VALUES ('admin@yahoo.com', 'admin','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin');

BEGIN TRANSACTION

CREATE TABLE [cookout] (
	cookout_id INT IDENTITY(1,1) NOT NULL,
	cookout_name varchar (50) NOT NULL,
	number_of_attendees int NOT NULL,
	host_id int NOT NULL,
	chef_id int NOT NULL,
	date_of_event DATETIME NOT NULL,
	street_address varchar(50) NOT NULL,
	city varchar(50) NOT NULL,
	state_abbreviation varchar(2) NOT NULL,
	zipcode int NOT NULL,
	description varchar(MAX) NOT NULL

	CONSTRAINT PK_cookout_id PRIMARY KEY (cookout_id),
	FOREIGN KEY (host_id) REFERENCES [users] (user_id),
	FOREIGN KEY (chef_id) REFERENCES [users] (user_id),
);

CREATE TABLE [menu_items] (
	item_id int IDENTITY (1,1) NOT NULL,
	name varchar(25) NOT NULL,
	description varchar(200) NOT NULL,
	price DECIMAL(18,2) NOT NULL CONSTRAINT DF_price DEFAULT(0),
	image varchar(MAX),
	is_vegan bit NOT NULL,
	is_vegetarian bit NOT NULL,
	is_gluten_free bit NOT NULL,
	is_available bit NOT NULL,
	is_guest_brought bit NOT NULL CONSTRAINT DF_is_guest_brought DEFAULT(0),
	cookout_id int NOT NULL,

	CONSTRAINT PK_menu_items PRIMARY KEY (item_id),
	FOREIGN KEY (cookout_id) REFERENCES [cookout] (cookout_id)
);

CREATE TABLE [orders] (
	order_id int IDENTITY (1,1) NOT NULL,
	cookout_id int NOT NULL,
	is_completed BIT NOT NULL CONSTRAINT DF_is_completed DEFAULT(0),
	email varchar(MAX) NOT NULL,

	CONSTRAINT PK_orders PRIMARY KEY (order_id),
	FOREIGN KEY (cookout_id) REFERENCES [cookout] (cookout_id),
);

CREATE TABLE [menu_order] (
	item_id int NOT NULL,
	order_id int NOT NULL,

	FOREIGN KEY (item_id) REFERENCES [menu_items] (item_id),
	FOREIGN KEY (order_id) REFERENCES [orders] (order_id),
);


CREATE TABLE [invites] (
	invite_id INT IDENTITY (1,1) NOT NULL,
	invite_email varchar(50) NOT NULL,
	person_name varchar(30) NULL,
	is_sent bit NOT NULL CONSTRAINT DF_is_sent DEFAULT(0),
	cookout_id int NOT NULL,
	response_status int NOT NULL CONSTRAINT DF_response_status DEFAULT(3),

	CONSTRAINT PK_invite_id PRIMARY KEY (invite_id),
	FOREIGN KEY (cookout_id) REFERENCES [cookout] (cookout_id),
);

CREATE TABLE [invite_user] (
	invite_id INT NOT NULL,
	user_id INT NOT NULL,

	FOREIGN KEY (invite_id) REFERENCES [invites] (invite_id),
	FOREIGN KEY (user_id) REFERENCES [users] (user_id)
);




INSERT INTO users (username, name, password_hash, salt, user_role) 
VALUES ('smithers1211@gmail.com', 'Nate', 'VidcTJ7WwNbp/PxNjTCwupuzu/4=', 'd7EznFSuk/E=', 'user'),
('jbakerp24@gmail.com', 'Joe', 'B2CQNT33D1VlAgFUIyYV6Q4u9Co=', 'LLb55cuD+nY=', 'user'),
('mcavinuejon@gmail.com', 'Jon', '8x5ge/e2FB3TZeHOHJfKdixy9mY=','0X+oeBaxunQ=',	'user'),
('duncan.aleah@gmail.com','AleahDuncan','AfzsYf1pei5InwJberYmsEkK1lA=','ccTutOgz84g=','user'),
('basset.bradlee@yahoo.com', 'BradleeBasset', 'rgRObW790N8JuUN571FynfDF+i8=','jLDXQMNlzjg=','user'),
('anderson.corbyn@techelevator.com', 'CorbynAnderson','y7szV40wM6odVe40NQpU3yQXqvg=','yRdHmWV/+pg=','user'),
('husik.daniel@techelevator.com', 'DanielHusik','ylmmIH5MRMvH0nyVyneYJSmGzEM=','uWfy44bxktE=','user'),
('dunn.geoffrey@mail.com', 'GeoffreyDunn','EjeGZXIEmMiyCvLc2m7q5nLv9nc=','47/QWpkyoQ8=','user'),
('davis.hannah@techelevator.com', 'HannahDavis','7E+Q2/X+j3j+KnIMq4p+PNDIzO8=','4UrCW64Mqto=','user'),
('slyk.hayden@gmail.com', 'HaydenSlyk','UCK2pcDcHhliryYhP+Wyd+0yyTg=','REEiBZg3cY0=','user'),
('averman.john@gmail.com', 'JohnAverman','0RyneaaftlOAeovKQ4+frtFkMCg=','rjBPAr+dYTU=','user'),
('taylor.kevin@techelevator.com', 'KevinTaylor','JLoYTmCgorrpiORStD6CJOodbKk=','xfDciVJqNYo=','user'),
('zeldin.matt@gmail.com', 'MattZeldin','Dut5jETLdnPFMXcf5T5kiqMv4K4=','HeYrYNDtMsA=','user'),
('arai.mio@gmail.com', 'MioArai','qma5mSc2KJIRVnE+JyesuasgS74=','FwIhNbKg22o=','user'),
('vashi.shivam@gmail.com', 'ShivamVashi','P1v3WdelyDiwKmzyeeqUykb75MI=','DE3bazfatnc=','user'),
('gyurgyik.vincent@yahoo.com', 'VincentGyurgyik','R/U8sTtQ3VOC4NHOdXymOFmfJ/E=','gXuubzkq3Io=','user'),
('calabrase.william@gmail.com','WilliamCalabrase','Gwy4sY1I1EW9gqxVlDkjly/E7hs=','Np7qwlpBTV8=','user')

INSERT INTO cookout (cookout_name, number_of_attendees,host_id,chef_id,date_of_event, street_address, city, state_abbreviation, zipcode, description)
VALUES ('Cookout Party', 3,8,4,'2023-08-30 15:15:00', '1261 Superior Ave', 'Cleveland', 'OH', 44114, 'Cookout for friends and family of the neighborhood'),
('Brian''s Birthday Bash', 10,6,6,'2023-09-18 13:00:00', '390 Euclid Avenue', 'Cleveland', 'OH', 44113, 'Brian turns 55 today! Lets celebrate with his favorite type of party!'),
('Brock''s Spectacular Extravaganza', 15,7,5,'2023-10-25 12:30:00', '2190 High Street', 'Columbus', 'OH', 43210, 'A fun filled time of burgers, dogs and ribs.'),
('Graduation Celebration', 16, 3, 3,'2023-08-18 16:00:00','7100 Euclid Avenue', 'Cleveland','OH',44103,'Congrats TE Graduates'),
('Nate''s Birthday Cookout',11,3,3,'2023-12-11 13:30:00','14532 Lake Avenue','Lakewood', 'OH',44107,'He will be upset if you do not show up!'),
('Fourth of July Block Party',20,3,4,'2024-07-04 14:00:00', '13538 ELbur Lane','Lakewood','OH',44107,'Celebrating Americas Birthday'),
('Super Bowl Sunday', 13, 11,12, '2024-02-11 14:00:00', '100 Main Street', 'Dallas', 'TX', 75001, 'Gather to watch the Superbowl!'),
('Memorial Day Cookout', 7,7,10,'2024-05-27 12:00:00', '50 Public Square', 'Cleveland','OH', 44106, 'Celebrate Memorial Day with Friends'),
('Christmas in July', 14, 5, 9, '2024-07-25 13:15:00', '120 W Goodale Street','Columbus','OH', 43215, 'Celebrate Christmas a little early')


INSERT INTO menu_items (name, description, price, image, is_vegan, is_vegetarian, is_gluten_free, is_available, cookout_id, is_guest_brought)
VALUES('Steak','A 8oz ribeye cooked medium-rare',10.99, 'https://greenvillejournal.com/wp-content/uploads/2020/07/Grilling-stock-1.jpg', 0,0,1,1,5,0),
('Grilled Vegetables', 'An assortment of grilled peppers, onions, mushrooms, tomatoes, zucchinni, and squash',2.49,'https://cf-images.us-east-1.prod.boltdns.net/v1/static/1033249144001/42161348-8d99-4cd6-9901-cb3cdf2df1a5/c8140d87-7c93-4e8e-acec-1fedb4c3eebf/1280x720/match/image.jpg',1,1,1,1,5,0),
('Hot Dog', 'A traditional ballpark frank grilled to perfection',1.50,'https://i.ytimg.com/vi/3bXN9YCJGz0/maxresdefault.jpg',0,0,0,1,5,0),
('Burger', 'A juicy 1/4 lb patty on a fresh sesame seed bun',4.99,'https://thekitchencommunity.org/wp-content/uploads/2020/12/Burger-Grill-Time-Chart-How-To-Grill-Burgers-1200x900.jpg',0,0,0,1,5,0),
('Corn on the Cob', 'A buttered, salted and beautifully charred corn on the cob', 3.99, 'https://hips.hearstapps.com/hmg-prod/images/shot-2-0129-1522854796.png?crop=1xw:1xh;center,top&resize=1200:*',1,1,1,1,5,0),
('Grilled Chicken','A grilled chicken breast',5.99,'https://www.eatingbirdfood.com/wp-content/uploads/2021/06/apple-cider-vinegar-chicken-grilling.jpg',0,0,1,1,4,0),
('Burger', 'A juicy 1/4 lb patty on a fresh sesame seed bun',4.99,'https://thekitchencommunity.org/wp-content/uploads/2020/12/Burger-Grill-Time-Chart-How-To-Grill-Burgers-1200x900.jpg',0,0,0,1,4,0),
('Hot Dog', 'A traditional ballpark frank grilled to perfection',1.50,'https://i.ytimg.com/vi/3bXN9YCJGz0/maxresdefault.jpg',0,0,0,1,4,0),
('Grilled Vegetables', 'An assortment of grilled peppers, onions, mushrooms, tomatoes, zucchinni, and squash',2.49,'https://cf-images.us-east-1.prod.boltdns.net/v1/static/1033249144001/42161348-8d99-4cd6-9901-cb3cdf2df1a5/c8140d87-7c93-4e8e-acec-1fedb4c3eebf/1280x720/match/image.jpg',1,1,1,1,4,0),
('Steak','A 8oz ribeye cooked medium-rare',10.99, 'https://greenvillejournal.com/wp-content/uploads/2020/07/Grilling-stock-1.jpg', 0,0,1,1,4,0),
('Smoked Brisket','A mouthwatering combination of savory, smoky, and slightly sweet flavors',13.99, 'https://houseofnasheats.com/wp-content/uploads/2022/09/Texas-Smoked-Beef-Brisket-Recipe-Squaree-1.jpg',0,0,1,1,4,0),
('Smoked Brisket','A mouthwatering combination of savory, smoky, and slightly sweet flavors',13.99, 'https://houseofnasheats.com/wp-content/uploads/2022/09/Texas-Smoked-Beef-Brisket-Recipe-Squaree-1.jpg',0,0,1,1,5,0),
('Smoked Wings','Rich, smokey and tender',6.99, 'https://disheswithdad.com/wp-content/uploads/2020/09/smoked-chicken-wings-1.jpg',0,0,1,1,4,0),
('Smoked Wings','Rich, smokey and tender',6.99, 'https://disheswithdad.com/wp-content/uploads/2020/09/smoked-chicken-wings-1.jpg',0,0,1,1,5,0),
('Grilled Shrimp','Incredibly juicy and flavorful grilled shrimp skewers with veggies',5.99,'https://littlesunnykitchen.com/wp-content/uploads/2021/06/Grilled-Shrimp-Kabobs-2.jpg',0,0,1,1,4,0),
('Grilled Shrimp','Incredibly juicy and flavorful grilled shrimp skewers with veggies',5.99,'https://littlesunnykitchen.com/wp-content/uploads/2021/06/Grilled-Shrimp-Kabobs-2.jpg',0,0,1,1,5,0),
('Grilled Watermelon','Sweet, salty, and smokey flavor that only leaves you wanting more.',4.99,'https://aarp-content.brightspotcdn.com/dims4/default/2c9ea39/2147483647/strip/false/crop/1280x704+0+0/resize/1280x704!/quality/90/?url=http%3A%2F%2Faarp-brightspot.s3.amazonaws.com%2Fcontent%2F9c%2F34%2Fa6ec36b743db9e24f6fbbede9cb8%2Fgrill-seasons-1280.jpg',1,1,1,1,4,0),
('Grilled Watermelon','Sweet, salty, and smokey flavor that only leaves you wanting more.',4.99,'https://aarp-content.brightspotcdn.com/dims4/default/2c9ea39/2147483647/strip/false/crop/1280x704+0+0/resize/1280x704!/quality/90/?url=http%3A%2F%2Faarp-brightspot.s3.amazonaws.com%2Fcontent%2F9c%2F34%2Fa6ec36b743db9e24f6fbbede9cb8%2Fgrill-seasons-1280.jpg',1,1,1,1,5,0)


INSERT INTO orders (cookout_id, is_completed, email)
VALUES (1,0,'jbakerp24@gmail.com'),
(2,1,'averman.john@gmail.com'),
(1,1,'calabrase.william@gmail.com'),
(2,0,'arai.mio@gmail.com'),
(5,0,'davis.hannah@techelevator.com'),
(5,0,'smithers1211@gmail.com'),
(5,0,'smithers1211@gmail.com'),
(5,1,'mcavinuejon@gmail.com'),
(4,0,'jbakerp24@gmail.com'),
(4,0,'mcavinuejon@gmail.com'),
(4,0,'duncan.aleah@gmail.com'),
(4,0,'basset.bradlee@yahoo.com'),
(4,0,'anderson.corbyn@techelevator.com'),
(4,0,'husik.daniel@techelevator.com'),
(4,0,'dunn.geoffrey@mail.com'),
(4,0,'davis.hannah@techelevator.com'),
(4,0, 'slyk.hayden@gmail.com'),
(4,0,'averman.john@gmail.com'),
(4,0,'taylor.kevin@techelevator.com'),
(4,0,'zeldin.matt@gmail.com'),
(4,0,'arai.mio@gmail.com'),
(4,0,'vashi.shivam@gmail.com'),
(4,0,'gyurgyik.vincent@yahoo.com'),
(4,0,'calabrase.william@gmail.com')

INSERT INTO menu_order (item_id, order_id)
VALUES (1,1),
(1,2),
(1,3),
(2,4),
(2,5),
(4,6),
(3,7),
(2,7),
(4,8),
(5,8),
(6,9),
(7,9),
(6,10),
(8,11),
(9,12),
(9,12),
(10,13),
(6,13),
(8,14),
(7,15),
(7,16),
(8,16),
(6,17),
(9,18),
(10,19),
(7,20),
(6,21),
(6,22),
(7,23),
(9,24)


INSERT INTO invites (invite_email, person_name, is_sent, cookout_id, response_status)
VALUES('Smithers1211@gmail.com', 'Nate Smith', 1, 1, 3),
('Smithers1211@gmail.com', 'Nate Smith', 1, 2, 1),
('Smithers1211@gmail.com', 'Nate Smith', 1, 3, 2),
('mcavinuejon@gmail.com', 'Jon', 1, 4, 2),
('jbakerp24@gmail.com','Joe',1,4,1),
('duncan.aleah@gmail.com','AleahDuncan',1,4,2),
('basset.bradlee@yahoo.com', 'BradleeBasset',1,4,3),
('anderson.corbyn@techelevator.com', 'CorbynAnderson',1,4,1),
('husik.daniel@techelevator.com', 'DanielHusik',1,4,1),
('dunn.geoffrey@mail.com', 'GeoffreyDunn',1,4,3),
('davis.hannah@techelevator.com', 'HannahDavis',1,4,1),
('slyk.hayden@gmail.com', 'HaydenSlyk',1,4,1),
('averman.john@gmail.com', 'JohnAverman',1,4,1),
('taylor.kevin@techelevator.com', 'KevinTaylor',1,4,1),
('zeldin.matt@gmail.com', 'MattZeldin',1,4,3),
('arai.mio@gmail.com', 'MioArai',1,4,1),
('vashi.shivam@gmail.com', 'ShivamVashi',1,4,1),
('gyurgyik.vincent@yahoo.com', 'VincentGyurgyik',1,4,3),
('calabrase.william@gmail.com','WilliamCalabrase',1,4,1),
('husik.daniel@techelevator.com', 'DanielHusik',1,5,1),
('dunn.geoffrey@mail.com', 'GeoffreyDunn',1,5,1),
('davis.hannah@techelevator.com', 'HannahDavis',1,5,1),
('slyk.hayden@gmail.com', 'HaydenSlyk',1,5,2),
('averman.john@gmail.com', 'JohnAverman',1,5,3),
('taylor.kevin@techelevator.com', 'KevinTaylor',1,5,1),
('zeldin.matt@gmail.com', 'MattZeldin',1,5,1),
('arai.mio@gmail.com', 'MioArai',1,5,1),
('vashi.shivam@gmail.com', 'ShivamVashi',1,5,1),
('gyurgyik.vincent@yahoo.com', 'VincentGyurgyik',1,5,3),
('calabrase.william@gmail.com','WilliamCalabrase',1,5,1),
('Smithers1211@gmail.com', 'Nate Smith', 1, 7, 1),
('Smithers1211@gmail.com', 'Nate Smith', 1, 8, 1),
('Smithers1211@gmail.com', 'Nate Smith', 1, 9, 1)
COMMIT;
