CREATE TABLE roles(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(256) NOT NULL
);

INSERT INTO roles("name") VALUES('worker');
INSERT INTO roles("name") VALUES('manager');
INSERT INTO roles("name") VALUES('director');
INSERT INTO roles("name") VALUES('superadmin');

CREATE TABLE company(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(256) NOT NULL,
	extra_data NTEXT
);

CREATE TABLE account(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    login NVARCHAR(256) NOT NULL,
    password NVARCHAR(256) NOT NULL,
    email NVARCHAR(256) NOT NULL,
    first_name NVARCHAR(256) NOT NULL,
    last_name NVARCHAR(256) NOT NULL
);

CREATE TABLE account_company_role(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    company_id INTEGER NOT NULL,
    role_id INTEGER,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(role_id) REFERENCES roles(id) ON UPDATE CASCADE ON DELETE SET NULL
);

CREATE TABLE room(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(256) NOT NULL,
    square FLOAT NOT NULL,
    height FLOAT NOT NULL,
    company_id INTEGER NOT NULL,
	extra_data NTEXT,
    FOREIGN KEY(company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE check_point(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    room_id INTEGER NOT NULL,
    room_other_id INTEGER NOT NULL,
    pass_condition NTEXT,
    pass_condition_api_url NTEXT,
	extra_data NTEXT,
	notify_check_api_url NTEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(room_other_id) REFERENCES room(id)
);

CREATE TABLE enter_point(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    room_id INTEGER NOT NULL,
    pass_condition NTEXT,
    pass_condition_api_url NTEXT,
	extra_data NTEXT,
	notify_enter_api_url NTEXT,
	notify_leave_api_url NTEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE interaction_point(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    room_id INTEGER NOT NULL,
    success_condition NTEXT,
    failure_condition NTEXT,
    interaction_api_url NTEXT,
	extra_data NTEXT,
	notify_success_api_url NTEXT,
	notify_failure_api_url NTEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE control_point(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    room_id INTEGER NOT NULL,
    violation_condition NTEXT,
    violation_api_url NTEXT,
	extra_data NTEXT,
	notify_violation_api_url NTEXT,
    FOREIGN KEY(room_id) REFERENCES room(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE enter_events(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    enter_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(enter_point_id) REFERENCES enter_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE leave_events(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    enter_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(enter_point_id) REFERENCES enter_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE check_events(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    check_point_id INTEGER NOT NULL,
    is_direct BIT NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(check_point_id) REFERENCES check_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE violation_events(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    control_point_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(control_point_id) REFERENCES control_point(id) ON UPDATE CASCADE ON DELETE NO ACTION
);

CREATE TABLE task(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    title NTEXT NOT NULL,
    description NTEXT NOT NULL,
    deadline DATETIME,
	extra_data NTEXT
);

CREATE TABLE task_position(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    task_id INTEGER NOT NULL,
    title NTEXT NOT NULL,
    description NTEXT NOT NULL,
    interaction_point_id INTEGER,
    alt_interaction_api_url NTEXT,
	alt_notify_success_api_url NTEXT,
	alt_notify_failure_api_url NTEXT,
    FOREIGN KEY(task_id) REFERENCES task(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE account_task(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    task_id INTEGER NOT NULL,
    deadline DATETIME,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY(task_id) REFERENCES task(id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE task_position_complete_events(
	id INTEGER PRIMARY KEY IDENTITY(1,1),
    account_id INTEGER NOT NULL,
    task_position_id INTEGER NOT NULL,
    date_time DATETIME NOT NULL,
    FOREIGN KEY(account_id) REFERENCES account(id) ON UPDATE CASCADE ON DELETE NO ACTION,
    FOREIGN KEY(task_position_id) REFERENCES task_position(id) ON UPDATE CASCADE ON DELETE NO ACTION
);