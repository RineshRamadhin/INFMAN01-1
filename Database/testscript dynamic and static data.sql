-- Schema INFMAN01_1
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS INFMAN01_1 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
-- -----------------------------------------------------
-- Schema INFMAN01_1
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Table `INFMAN01_1`.`garages`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `INFMAN01_1`.`garages` ;

CREATE TABLE IF NOT EXISTS `INFMAN01_1`.`garages` (
	`identifier` 		VARCHAR(45) 	NOT NULL ,
	`name` 				VARCHAR(255) 	NOT NULL ,
	`latitude` 			DECIMAL(20,15)	NOT NULL ,
	`longitude` 		DECIMAL(20,15)	NOT NULL ,
	`last updated` 		DATETIME 		NOT NULL ,
	PRIMARY KEY (`identifier`)  );

-- -----------------------------------------------------
-- Table `INFMAN01_1`.`garagedata`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `INFMAN01_1`.`garagedata` ;

CREATE TABLE IF NOT EXISTS `INFMAN01_1`.`garagedata` (
	`identifier` 		VARCHAR(45) 	NOT NULL ,
	`description` 		VARCHAR(255) 	NOT NULL ,
	`open` 				BOOLEAN			NOT NULL ,
	`full` 				BOOLEAN 		NOT NULL ,
	`parking capacity` 	INT		 		NOT NULL ,
	`vacant spaces` 	INT 			NOT NULL ,
	`last updated` 		DATETIME 		NOT NULL ,
    FOREIGN KEY (`identifier`) REFERENCES garages(`identifier`)
    );
    
