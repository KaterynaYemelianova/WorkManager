CREATE TABLE roles(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(256) NOT NULL
);

INSERT INTO roles VALUES(0, 'worker');
INSERT INTO roles VALUES(0, 'manager');
INSERT INTO roles VALUES(0, 'director');
INSERT INTO roles VALUES(0, 'superadmin');

CREATE TABLE company(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(256) NOT NULL,
	extra_data TEXT
);

CREATE TABLE account(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    login VARCHAR(256) NOT NULL,
    password VARCHAR(256) NOT NULL,
    email VARCHAR(256) NOT NULL,
    first_name VARCHAR(256) NOT NULL,
    last_name VARCHAR(256) NOT NULL
);

CREATE TABLE account_company_role(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    company_id INTEGER NOT NULL,
    role_id INTEGER,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(role_id) REFERENCES roles(id) ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE TABLE room(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(256) NOT NULL,
    square FLOAT NOT NULL,
    height FLOAT NOT NULL,
    company_id INTEGER NOT NULL,
	extra_data TEXT,
    FOREIGN KEY(company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE check_point(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    room_id INTEGER NOT NULL,
    room_other_id INTEGER NOT NULL,
    pass_condition TEXT,
    pass_condition_api_url TEXT,
	extra_data TEXT,
	notify_check_api_url TEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(room_other_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE enter_point(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    room_id INTEGER NOT NULL,
    pass_condition TEXT,
    pass_condition_api_url TEXT,
	extra_data TEXT,
	notify_enter_api_url TEXT,
	notify_leave_api_url TEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE interaction_point(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    room_id INTEGER NOT NULL,
    success_condition TEXT,
    failure_condition TEXT,
    interaction_api_url TEXT,
	extra_data TEXT,
	notify_success_api_url TEXT,
	notify_failure_api_url TEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE control_point(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    room_id INTEGER NOT NULL,
    violation_condition TEXT,
    violation_api_url TEXT,
	extra_data TEXT,
	notify_violation_api_url TEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE enter_events(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    enter_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(enter_point_id) REFERENCES enter_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE leave_events(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    enter_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(enter_point_id) REFERENCES enter_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE check_events(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    check_point_id INTEGER NOT NULL,
    is_direct BOOLEAN NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(check_point_id) REFERENCES check_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE violation_events(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    control_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(control_point_id) REFERENCES control_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE task(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    title TEXT NOT NULL,
    description TEXT NOT NULL,
    deadline DATETIME,
	extra_data TEXT
);

CREATE TABLE task_position(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    task_id INTEGER NOT NULL,
    title TEXT NOT NULL,
    description TEXT NOT NULL,
    interaction_point_id INTEGER,
    alt_interaction_api_url TEXT,
	alt_notify_success_api_url TEXT,
	alt_notify_failure_api_url TEXT,
    FOREIGN KEY(task_id) REFERENCES task(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE account_task(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    task_id INTEGER NOT NULL,
    deadline DATETIME,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(task_id) REFERENCES task(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE task_position_complete_events(
	id INTEGER PRIMARY KEY AUTO_INCREMENT,
    account_id INTEGER NOT NULL,
    task_position_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(task_position_id) REFERENCES task_position(id) ON UPDATE CASCADE ON DELETE NO ACTION
);