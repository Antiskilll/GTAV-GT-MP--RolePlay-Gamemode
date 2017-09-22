/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50711
Source Host           : localhost:3306
Source Database       : gtarp

Target Server Type    : MYSQL
Target Server Version : 50711
File Encoding         : 65001

Date: 2017-09-21 18:22:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `concessionnaire`
-- ----------------------------
DROP TABLE IF EXISTS `concessionnaire`;
CREATE TABLE `concessionnaire` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `vehiclehash` text NOT NULL,
  `name` text NOT NULL,
  `nameconcess` text NOT NULL,
  `poids` int(10) NOT NULL,
  `price` int(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=207 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of concessionnaire
-- ----------------------------
INSERT INTO `concessionnaire` VALUES ('1', '841808271', 'Rhapsody', 'Compact Cars', '30', '14600');
INSERT INTO `concessionnaire` VALUES ('2', '1549126457', 'Brioso R/A', 'Compact Cars', '30', '6830');
INSERT INTO `concessionnaire` VALUES ('3', '-1450650718', 'Prairie', 'Compact Cars', '30', '53200');
INSERT INTO `concessionnaire` VALUES ('4', '-1130810103', 'Dilettante', 'Compact Cars', '30', '8600');
INSERT INTO `concessionnaire` VALUES ('5', '-431692672', 'Panto', 'Compact Cars', '30', '4200');
INSERT INTO `concessionnaire` VALUES ('6', '-344943009', 'Blista', 'Compact Cars', '25', '3600');
INSERT INTO `concessionnaire` VALUES ('7', '-591651781', 'Go Go Monkey Blista', 'Compact Cars', '30', '14140');
INSERT INTO `concessionnaire` VALUES ('8', '330661258', 'Cognoscenti Cabrio', 'Coupes', '25', '67650');
INSERT INTO `concessionnaire` VALUES ('9', '1349725314', 'Sentinel XS', 'Coupes', '25', '99470');
INSERT INTO `concessionnaire` VALUES ('10', '-511601230', 'Oracle', 'Coupes', '25', '65890');
INSERT INTO `concessionnaire` VALUES ('11', '873639469', 'Sentinel', 'Coupes', '25', '72520');
INSERT INTO `concessionnaire` VALUES ('12', '1581459400', 'Windsor', 'Coupes', '25', '109220');
INSERT INTO `concessionnaire` VALUES ('13', '-1930048799', 'Windsor Drop', 'Coupes', '25', '121410');
INSERT INTO `concessionnaire` VALUES ('14', '-1193103848', 'Zion Cabrio', 'Coupes', '25', '95540');
INSERT INTO `concessionnaire` VALUES ('15', '-1122289213', 'Zion', 'Coupes', '25', '99540');
INSERT INTO `concessionnaire` VALUES ('16', '-624529134', 'Jackal', 'Coupes', '25', '74140');
INSERT INTO `concessionnaire` VALUES ('17', '-591610296', 'F620', 'Coupes', '25', '150760');
INSERT INTO `concessionnaire` VALUES ('18', '1348744438', 'Oracle XS', 'Coupes', '25', '52650');
INSERT INTO `concessionnaire` VALUES ('19', '-391594584', 'Felon', 'Coupes', '25', '68600');
INSERT INTO `concessionnaire` VALUES ('20', '-89291282', 'Felon GT', 'Coupes', '25', '96440');
INSERT INTO `concessionnaire` VALUES ('21', '-5153954', 'Exemplar', 'Coupes', '25', '86160');
INSERT INTO `concessionnaire` VALUES ('22', '-808831384', 'Baller', 'SUV', '80', '142520');
INSERT INTO `concessionnaire` VALUES ('23', '486987393', 'Huntley S', 'SUV', '80', '112520');
INSERT INTO `concessionnaire` VALUES ('24', '1878062887', 'Baller LE', 'SUV', '80', '152520');
INSERT INTO `concessionnaire` VALUES ('25', '884422927', 'Habanero', 'SUV', '75', '94512');
INSERT INTO `concessionnaire` VALUES ('26', '1177543287', 'Dubsta', 'SUV', '80', '125900');
INSERT INTO `concessionnaire` VALUES ('27', '1203490606', 'XLS', 'SUV', '80', '120040');
INSERT INTO `concessionnaire` VALUES ('28', '1221512915', 'Seminole', 'SUV', '80', '100680');
INSERT INTO `concessionnaire` VALUES ('29', '1269098716', 'Landstalker', 'SUV', '80', '180140');
INSERT INTO `concessionnaire` VALUES ('30', '1337041428', 'Serrano', 'SUV', '70', '81000');
INSERT INTO `concessionnaire` VALUES ('31', '2136773105', 'Rocoto', 'SUV', '80', '100260');
INSERT INTO `concessionnaire` VALUES ('32', '-1651067813', 'Radius', 'SUV', '80', '127300');
INSERT INTO `concessionnaire` VALUES ('33', '-1543762099', 'Gresley', 'SUV', '80', '216600');
INSERT INTO `concessionnaire` VALUES ('34', '-1137532101', 'FQ 2', 'SUV', '80', '154570');
INSERT INTO `concessionnaire` VALUES ('35', '-808831384', 'Baller', 'SUV', '80', '228660');
INSERT INTO `concessionnaire` VALUES ('36', '-808457413', 'Patriot', 'SUV', '90', '406600');
INSERT INTO `concessionnaire` VALUES ('37', '2006918058', 'Cavalcade', 'SUV', '80', '136600');
INSERT INTO `concessionnaire` VALUES ('38', '914654722', 'Mesa', 'SUV', '80', '160230');
INSERT INTO `concessionnaire` VALUES ('39', '-498054846', 'Virgo', 'Muscle Cars', '60', '165360');
INSERT INTO `concessionnaire` VALUES ('40', '37348240', 'Hotknife', 'Muscle Cars', '60', '281000');
INSERT INTO `concessionnaire` VALUES ('41', '80636076', 'Dominator', 'Muscle Cars', '60', '353520');
INSERT INTO `concessionnaire` VALUES ('42', '-1685021548', 'Sabre Turbo', 'Muscle Cars', '60', '277830');
INSERT INTO `concessionnaire` VALUES ('43', '349605904', 'Chino', 'Muscle Cars', '60', '120720');
INSERT INTO `concessionnaire` VALUES ('44', '2006667053', 'Voodoo Custom', 'Muscle Cars', '60', '438760');
INSERT INTO `concessionnaire` VALUES ('45', '525509695', 'Moonbeam', 'Muscle Cars', '45', '51660');
INSERT INTO `concessionnaire` VALUES ('46', '723973206', 'Dukes', 'Muscle Cars', '60', '280240');
INSERT INTO `concessionnaire` VALUES ('47', '729783779', 'Slamvan', 'Muscle Cars', '60', '97330');
INSERT INTO `concessionnaire` VALUES ('48', '972671128', 'Tampa', 'Muscle Cars', '60', '248040');
INSERT INTO `concessionnaire` VALUES ('49', '833469436', 'Lost Slamvan', 'Muscle Cars', '60', '90000');
INSERT INTO `concessionnaire` VALUES ('50', '1507916787', 'Picador', 'Muscle Cars', '45', '52800');
INSERT INTO `concessionnaire` VALUES ('51', '1896491931', 'Moonbeam Custom', 'Muscle Cars', '60', '75784');
INSERT INTO `concessionnaire` VALUES ('52', '523724515', 'Voodoo', 'Muscle Cars', '60', '42120');
INSERT INTO `concessionnaire` VALUES ('53', '2068293287', 'Lurcher', 'Muscle Cars', '60', '81780');
INSERT INTO `concessionnaire` VALUES ('54', '-2119578145', 'Faction', 'Muscle Cars', '60', '87280');
INSERT INTO `concessionnaire` VALUES ('55', '-2095439403', 'Phoenix', 'Muscle Cars', '60', '228960');
INSERT INTO `concessionnaire` VALUES ('56', '-1790546981', 'Faction Custom', 'Muscle Cars', '60', '97280');
INSERT INTO `concessionnaire` VALUES ('57', '-1943285540', 'Nightshade', 'Muscle Cars', '60', '260000');
INSERT INTO `concessionnaire` VALUES ('58', '-1800170043', 'Gauntlet', 'Muscle Cars', '60', '314600');
INSERT INTO `concessionnaire` VALUES ('59', '-1790546981', 'Faction Custom', 'Muscle Cars', '60', '97280');
INSERT INTO `concessionnaire` VALUES ('60', '-1685021548', 'Sabre Turbo', 'Muscle Cars', '60', '77280');
INSERT INTO `concessionnaire` VALUES ('61', '-1361687965', 'Chino Custom', 'Muscle Cars', '60', '325360');
INSERT INTO `concessionnaire` VALUES ('62', '-1205801634', 'Blade', 'Muscle Cars', '60', '181640');
INSERT INTO `concessionnaire` VALUES ('64', '16646064', 'Virgo Classic', 'Muscle Cars', '60', '225570');
INSERT INTO `concessionnaire` VALUES ('65', '-825837129', 'Vigero', 'Muscle Cars', '60', '348040');
INSERT INTO `concessionnaire` VALUES ('66', '-682211828', 'Buccaneer', 'Muscle Cars', '60', '277280');
INSERT INTO `concessionnaire` VALUES ('67', '-667151410', 'Rat-Loader', 'Muscle Cars', '40', '48840');
INSERT INTO `concessionnaire` VALUES ('68', '-589178377', 'Rat-Truck', 'Muscle Cars', '40', '86160');
INSERT INTO `concessionnaire` VALUES ('71', '-227741703', 'Ruiner', 'Muscle Cars', '60', '280640');
INSERT INTO `concessionnaire` VALUES ('72', '108773431', 'Coquette', 'Sports Cars', '15', '1500000');
INSERT INTO `concessionnaire` VALUES ('73', '384071873', 'Surano', 'Sports Cars', '15', '1450000');
INSERT INTO `concessionnaire` VALUES ('74', '482197771', 'Lynx', 'Sports Cars', '15', '1700350');
INSERT INTO `concessionnaire` VALUES ('75', '499169875', 'Fusilade', 'Sports Cars', '15', '1000000');
INSERT INTO `concessionnaire` VALUES ('76', '544021352', 'Khamelion', 'Sports Cars', '15', '900000');
INSERT INTO `concessionnaire` VALUES ('77', '767087018', 'Alpha', 'Sports Cars', '15', '875000');
INSERT INTO `concessionnaire` VALUES ('78', '970598228', 'Sultan', 'Sports Cars', '15', '750000');
INSERT INTO `concessionnaire` VALUES ('79', '2016857647', 'Futo', 'Sports Cars', '15', '250000');
INSERT INTO `concessionnaire` VALUES ('80', '2072687711', 'Carbonizzare', 'Sports Cars', '15', '1200000');
INSERT INTO `concessionnaire` VALUES ('81', '-1372848492', 'Kuruma', 'Sports Cars', '15', '3000000');
INSERT INTO `concessionnaire` VALUES ('82', '-1297672541', 'Jester', 'Sports Cars', '15', '2500000');
INSERT INTO `concessionnaire` VALUES ('83', '-1041692462', 'Banshee', 'Sports Cars', '15', '1600000');
INSERT INTO `concessionnaire` VALUES ('84', '-777172681', 'Omnis', 'Sports Cars', '15', '850000');
INSERT INTO `concessionnaire` VALUES ('85', '-377465520', 'Penumbra', 'Sports Cars', '15', '1750000');
INSERT INTO `concessionnaire` VALUES ('86', '-304802106', 'Buffalo', 'Sports Cars', '15', '1550000');
INSERT INTO `concessionnaire` VALUES ('87', '-142942670', 'Massacro', 'Sports Cars', '15', '2600000');
INSERT INTO `concessionnaire` VALUES ('88', '234062309', 'Reaper', 'Supercars', '15', '3500000');
INSERT INTO `concessionnaire` VALUES ('89', '338562499', 'Vacca', 'Supercars', '15', '3000000');
INSERT INTO `concessionnaire` VALUES ('90', '408192225', 'Turismo R', 'Supercars', '15', '5000000');
INSERT INTO `concessionnaire` VALUES ('91', '418536135', 'Infernus', 'Supercars', '15', '2850000');
INSERT INTO `concessionnaire` VALUES ('92', '633712403', 'Banshee 900R', 'Supercars', '15', '3600000');
INSERT INTO `concessionnaire` VALUES ('93', '1426219628', 'FMJ', 'Supercars', '15', '5000000');
INSERT INTO `concessionnaire` VALUES ('94', '1663218586', 'T20', 'Supercars', '15', '6500000');
INSERT INTO `concessionnaire` VALUES ('95', '1987142870', 'Osiris', 'Supercars', '15', '6000000');
INSERT INTO `concessionnaire` VALUES ('96', '2067820283', 'Tyrus', 'Supercars', '15', '8000000');
INSERT INTO `concessionnaire` VALUES ('97', '2123327359', 'X80 Proto', 'Supercars', '15', '8500000');
INSERT INTO `concessionnaire` VALUES ('98', '-1829802492', '811', 'Supercars', '15', '4500999');
INSERT INTO `concessionnaire` VALUES ('99', '-1696146015', 'Bullet', 'Supercars', '15', '2600000');
INSERT INTO `concessionnaire` VALUES ('100', '-1622444098', 'Voltic', 'Supercars', '15', '3250000');
INSERT INTO `concessionnaire` VALUES ('101', '-1403128555', 'Zentorno', 'Supercars', '15', '10000000');
INSERT INTO `concessionnaire` VALUES ('102', '-1311154784', 'Cheetah', 'Supercars', '15', '8500000');
INSERT INTO `concessionnaire` VALUES ('103', '-1291952903', 'Entity XF', 'Supercars', '15', '9000000');
INSERT INTO `concessionnaire` VALUES ('104', '-1232836011', 'RE-7B', 'Supercars', '15', '11000000');
INSERT INTO `concessionnaire` VALUES ('105', '-1216765807', 'Adder', 'Supercars', '15', '8500000');
INSERT INTO `concessionnaire` VALUES ('106', '-295689028', 'Sultan RS', 'Supercars', '15', '6000000');
INSERT INTO `concessionnaire` VALUES ('107', '86520421', 'BF400', 'Moto', '5', '45175');
INSERT INTO `concessionnaire` VALUES ('108', '301427732', 'Hexer', 'Moto', '5', '125900');
INSERT INTO `concessionnaire` VALUES ('109', '390201602', 'Cliffhanger', 'Moto', '5', '77910');
INSERT INTO `concessionnaire` VALUES ('110', '640818791', 'Lectro', 'Moto', '5', '87200');
INSERT INTO `concessionnaire` VALUES ('111', '741090084', 'Gargoyle', 'Moto', '5', '195001');
INSERT INTO `concessionnaire` VALUES ('112', '743478836', 'Sovereign', 'Moto', '5', '229400');
INSERT INTO `concessionnaire` VALUES ('113', '1265391242', 'Hakuchou', 'Moto', '5', '247170');
INSERT INTO `concessionnaire` VALUES ('114', '1672195559', 'Akuma', 'Moto', '5', '146000');
INSERT INTO `concessionnaire` VALUES ('115', '1753414259', 'Enduro', 'Moto', '5', '58500');
INSERT INTO `concessionnaire` VALUES ('116', '1836027715', 'Thrust', 'Moto', '5', '84920');
INSERT INTO `concessionnaire` VALUES ('117', '2006142190', 'Daemon', 'Moto', '5', '145900');
INSERT INTO `concessionnaire` VALUES ('118', '-2140431165', 'Bagger', 'Moto', '5', '238850');
INSERT INTO `concessionnaire` VALUES ('119', '788045382', 'Sanchez (livery)', 'Moto', '5', '54600');
INSERT INTO `concessionnaire` VALUES ('120', '-1353081087', 'Vindicator', 'Moto', '5', '114430');
INSERT INTO `concessionnaire` VALUES ('121', '-893578776', 'Ruffian', 'Moto', '5', '158200');
INSERT INTO `concessionnaire` VALUES ('122', '-634879114', 'Nemesis', 'Moto', '5', '167500');
INSERT INTO `concessionnaire` VALUES ('123', '-159126838', 'Innovation', 'Moto', '5', '572000');
INSERT INTO `concessionnaire` VALUES ('124', '-140902153', 'Vader', 'Moto', '5', '256700');
INSERT INTO `concessionnaire` VALUES ('125', '65402552', 'Youga', 'Vans', '65', '92400');
INSERT INTO `concessionnaire` VALUES ('127', '-1987130134', 'Boxville', 'Vans', '65', '81600');
INSERT INTO `concessionnaire` VALUES ('133', '1488164764', 'Paradise', 'Vans', '65', '82920');
INSERT INTO `concessionnaire` VALUES ('134', '-16948145', 'Bison', 'Vans', '65', '144800');
INSERT INTO `concessionnaire` VALUES ('143', '-810318068', 'Speedo', 'Vans', '65', '114040');
INSERT INTO `concessionnaire` VALUES ('146', '-120287622', 'Journey', 'Vans', '35', '11320');
INSERT INTO `concessionnaire` VALUES ('149', '231083307', 'Speeder', 'Bateau', '0', '3520000');
INSERT INTO `concessionnaire` VALUES ('150', '1033245328', 'Dinghy', 'Bateau', '0', '3280000');
INSERT INTO `concessionnaire` VALUES ('151', '290013743', 'Tropic', 'Bateau', '0', '2470000');
INSERT INTO `concessionnaire` VALUES ('152', '400514754', 'Squalo', 'Bateau', '0', '2070000');
INSERT INTO `concessionnaire` VALUES ('153', '231083307', 'Speeder', 'Bateau', '0', '3520000');
INSERT INTO `concessionnaire` VALUES ('154', '1033245328', 'Dinghy', 'Bateau', '0', '3280000');
INSERT INTO `concessionnaire` VALUES ('155', '771711535', 'Submersible', 'Bateau', '0', '1000000');
INSERT INTO `concessionnaire` VALUES ('156', '861409633', 'Jetmax', 'Bateau', '0', '3825000');
INSERT INTO `concessionnaire` VALUES ('157', '1033245328', 'Dinghy', 'Bateau', '0', '3280000');
INSERT INTO `concessionnaire` VALUES ('158', '1070967343', 'Toro', 'Bateau', '0', '3960000');
INSERT INTO `concessionnaire` VALUES ('159', '1033245328', 'Dinghy', 'Bateau', '0', '3280000');
INSERT INTO `concessionnaire` VALUES ('160', '1070967343', 'Toro', 'Bateau', '0', '3960000');
INSERT INTO `concessionnaire` VALUES ('161', '290013743', 'Tropic', 'Bateau', '0', '2470000');
INSERT INTO `concessionnaire` VALUES ('162', '-2100640717', 'Tug', 'Bateau', '0', '66000');
INSERT INTO `concessionnaire` VALUES ('163', '-1043459709', 'Marquis', 'Bateau', '0', '125000');
INSERT INTO `concessionnaire` VALUES ('164', '-1030275036', 'Seashark', 'Bateau', '0', '2687500');
INSERT INTO `concessionnaire` VALUES ('165', '-1030275036', 'Seashark', 'Bateau', '0', '2687500');
INSERT INTO `concessionnaire` VALUES ('166', '-1030275036', 'Seashark', 'Bateau', '0', '2687500');
INSERT INTO `concessionnaire` VALUES ('167', '-282946103', 'Suntrap', 'Bateau', '0', '2070000');
INSERT INTO `concessionnaire` VALUES ('168', '710198397', 'SuperVolito', 'Hélicoptère', '0', '17186750');
INSERT INTO `concessionnaire` VALUES ('169', '744705981', 'Frogger', 'Hélicoptère', '0', '15366400');
INSERT INTO `concessionnaire` VALUES ('170', '788747387', 'Buzzard Attack Chopper', 'Hélicoptère', '0', '15361500');
INSERT INTO `concessionnaire` VALUES ('171', '837858166', 'Annihilator', 'Hélicoptère', '0', '10824100');
INSERT INTO `concessionnaire` VALUES ('172', '1044954915', 'Skylift', 'Hélicoptère', '0', '11054400');
INSERT INTO `concessionnaire` VALUES ('173', '-50547061', 'Cargobob', 'Hélicoptère', '0', '12245100');
INSERT INTO `concessionnaire` VALUES ('174', '-50547061', 'Cargobob', 'Hélicoptère', '0', '12245100');
INSERT INTO `concessionnaire` VALUES ('175', '744705981', 'Frogger', 'Hélicoptère', '0', '15366400');
INSERT INTO `concessionnaire` VALUES ('176', '-50547061', 'Cargobob', 'Hélicoptère', '0', '12245100');
INSERT INTO `concessionnaire` VALUES ('177', '-1845487887', 'Volatus', 'Hélicoptère', '0', '15900500');
INSERT INTO `concessionnaire` VALUES ('178', '-1660661558', 'Maverick', 'Hélicoptère', '0', '13759190');
INSERT INTO `concessionnaire` VALUES ('179', '-1600252419', 'Valkyrie', 'Hélicoptère', '0', '14023790');
INSERT INTO `concessionnaire` VALUES ('180', '-339587598', 'Swift', 'Hélicoptère', '0', '15773100');
INSERT INTO `concessionnaire` VALUES ('181', '-82626025', 'Savage', 'Hélicoptère', '0', '14905800');
INSERT INTO `concessionnaire` VALUES ('182', '-50547061', 'Cargobob', 'Hélicoptère', '0', '12245100');
INSERT INTO `concessionnaire` VALUES ('183', '165154707', 'Miljet', 'Avion', '0', '37009690');
INSERT INTO `concessionnaire` VALUES ('184', '621481054', 'Luxor', 'Avion', '0', '35721000');
INSERT INTO `concessionnaire` VALUES ('185', '970356638', 'Duster', 'Avion', '0', '16905000');
INSERT INTO `concessionnaire` VALUES ('186', '970385471', 'Hydra', 'Avion', '0', '48068990');
INSERT INTO `concessionnaire` VALUES ('187', '1058115860', 'Jet', 'Avion', '0', '24078600');
INSERT INTO `concessionnaire` VALUES ('188', '1341619767', 'Vestra', 'Avion', '0', '43727600');
INSERT INTO `concessionnaire` VALUES ('189', '1824333165', 'Besra', 'Avion', '0', '91654500');
INSERT INTO `concessionnaire` VALUES ('190', '1981688531', 'Titan', 'Avion', '0', '23696400');
INSERT INTO `concessionnaire` VALUES ('191', '-1746576111', 'Mammatus', 'Avion', '0', '16905000');
INSERT INTO `concessionnaire` VALUES ('192', '-1673356438', 'Velum', 'Avion', '0', '20668200');
INSERT INTO `concessionnaire` VALUES ('193', '-1295027632', 'Nimbus', 'Avion', '0', '38229790');
INSERT INTO `concessionnaire` VALUES ('194', '-1214505995', 'Shamal', 'Avion', '0', '35721000');
INSERT INTO `concessionnaire` VALUES ('195', '-901163259', 'Dodo', 'Avion', '0', '16905000');
INSERT INTO `concessionnaire` VALUES ('196', '569305213', 'Packer', 'Camion', '200', '34020');
INSERT INTO `concessionnaire` VALUES ('197', '850991848', 'Biff', 'Camion', '200', '18360');
INSERT INTO `concessionnaire` VALUES ('198', '904750859', 'Mule', 'Camion', '200', '13860');
INSERT INTO `concessionnaire` VALUES ('199', '1518533038', 'Hauler', 'Camion', '200', '20160');
INSERT INTO `concessionnaire` VALUES ('200', '1747439474', 'Stockade', 'Camion', '200', '18360');
INSERT INTO `concessionnaire` VALUES ('201', '2053223216', 'Benson', 'Camion', '200', '25920');
INSERT INTO `concessionnaire` VALUES ('202', '2112052861', 'Pounder', 'Camion', '200', '20790');
INSERT INTO `concessionnaire` VALUES ('203', '-2137348917', 'Phantom', 'Camion', '200', '31180');
INSERT INTO `concessionnaire` VALUES ('204', '904750859', 'Mule', 'Camion', '200', '23710');
INSERT INTO `concessionnaire` VALUES ('205', '904750859', 'Mule', 'Camion', '200', '13860');
INSERT INTO `concessionnaire` VALUES ('206', '1747439474', 'Stockade', 'Camion', '200', '18360');

-- ----------------------------
-- Table structure for `housing`
-- ----------------------------
DROP TABLE IF EXISTS `housing`;
CREATE TABLE `housing` (
  `ID` int(5) NOT NULL,
  `Owner` varchar(50) NOT NULL,
  `Inventory` varchar(50) CHARACTER SET utf8 NOT NULL,
  `Dimension` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of housing
-- ----------------------------

-- ----------------------------
-- Table structure for `jobs`
-- ----------------------------
DROP TABLE IF EXISTS `jobs`;
CREATE TABLE `jobs` (
  `name` varchar(9999) DEFAULT NULL,
  `owner` varchar(9999) DEFAULT NULL,
  `member` varchar(9999) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of jobs
-- ----------------------------

-- ----------------------------
-- Table structure for `licences`
-- ----------------------------
DROP TABLE IF EXISTS `licences`;
CREATE TABLE `licences` (
  `owner` varchar(70) NOT NULL,
  `voiture` tinyint(4) NOT NULL,
  `moto` tinyint(1) NOT NULL DEFAULT '0',
  `poidslourd` tinyint(1) NOT NULL DEFAULT '0',
  `taxi` tinyint(1) NOT NULL DEFAULT '0',
  `pizza` tinyint(1) NOT NULL DEFAULT '0',
  `fourriere` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`owner`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of licences
-- ----------------------------

-- ----------------------------
-- Table structure for `user`
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `name` varchar(20) NOT NULL DEFAULT '',
  `money` int(20) NOT NULL,
  `ip` varchar(255) NOT NULL,
  `group` int(2) NOT NULL DEFAULT '0',
  `bank` int(10) NOT NULL,
  `jailed` tinyint(1) NOT NULL DEFAULT '0',
  `inventory` text NOT NULL,
  `position` text NOT NULL,
  `characters` text NOT NULL,
  `adminRank` int(20) NOT NULL,
  `LSPDrank` int(20) NOT NULL DEFAULT '0',
  `clothing` varchar(255) NOT NULL DEFAULT '[[0],[0],[0],[0],[0]]',
  `hunger` int(3) NOT NULL DEFAULT '100',
  `thirst` int(3) NOT NULL DEFAULT '100',
  `health` int(3) NOT NULL DEFAULT '100',
  `EMSrank` int(20) NOT NULL DEFAULT '0',
  `rpname` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------

-- ----------------------------
-- Table structure for `vehicles`
-- ----------------------------
DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE `vehicles` (
  `id` int(6) NOT NULL AUTO_INCREMENT,
  `side` varchar(16) NOT NULL,
  `classname` varchar(64) NOT NULL,
  `type` varchar(16) NOT NULL,
  `pid` varchar(17) NOT NULL,
  `plate` varchar(8) NOT NULL,
  `color` varchar(20) NOT NULL,
  `inventory` varchar(64) NOT NULL,
  `fuel` double NOT NULL DEFAULT '1',
  `rotation` varchar(64) NOT NULL,
  `position` varchar(256) NOT NULL,
  `active` int(1) NOT NULL,
  `insert_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_time` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`active`),
  KEY `side` (`side`),
  KEY `pid` (`pid`),
  KEY `type` (`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
-- Records of vehicles
-- ----------------------------
