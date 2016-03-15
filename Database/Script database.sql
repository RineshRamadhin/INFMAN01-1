-- Schema GARAGES
-- -----------------------------------------------------
DROP SCHEMA GARAGES;

CREATE SCHEMA IF NOT EXISTS GARAGES DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;

-- -----------------------------------------------------
-- Table `GARAGES`.`info`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GARAGES`.`info` ;
 

CREATE TABLE IF NOT EXISTS `GARAGES`.`info` (
 `garage_id` 		VARCHAR(45) 	NOT NULL ,
 `name` 			VARCHAR(255) 	NOT NULL ,
 `latitude` 		DECIMAL(20,15) 	NOT NULL ,
 `longitude` 		DECIMAL(20,15) 	NOT NULL ,
 `aantal_plekken` 	INT 			NOT NULL ,
 PRIMARY KEY (`garage_id`) );
 

-- -----------------------------------------------------
-- Table `GARAGES`.`datum`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GARAGES`.`datum` ;
 

CREATE TABLE IF NOT EXISTS `GARAGES`.`datum` (
-- `datum_id` 	VARCHAR(12) 	NOT NULL ,
 `datum` 		TIMESTAMP 		NOT NULL ,
 `dag` 			VARCHAR(9) 		NOT NULL ,
 `maand` 		VARCHAR(15) 	NOT NULL ,
 `jaar` 		VARCHAR(4) 		NOT NULL ,
 `uur` 			VARCHAR(2) 		NOT NULL ,
 `minuut` 		VARCHAR(2) 		NOT NULL ,
-- PRIMARY KEY (`datum_id`) );
 PRIMARY KEY (`datum`) );
 

-- -----------------------------------------------------
-- Table `GARAGES`.`feit`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GARAGES`.`feit` ;
 

CREATE TABLE IF NOT EXISTS `GARAGES`.`feit` (
 `garage_id` 		VARCHAR(45) 	NOT NULL ,
-- `datum_id` 		VARCHAR(12) 	NOT NULL ,
 `datum` 			TIMESTAMP	 	NOT NULL ,
 `open` 			BOOLEAN			NOT NULL ,
 `full` 			BOOLEAN 		NOT NULL ,
 `vrije_plekken` 	INT 			NOT NULL ,
 FOREIGN KEY (`garage_id`) 	REFERENCES GARAGES.info(`garage_id`),
-- FOREIGN KEY (`datum_id`) 	REFERENCES GARAGES.datum(`datum_id`)
 FOREIGN KEY (`datum`) 		REFERENCES GARAGES.datum(`datum`)
 );