CREATE TABLE service (
  [service_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [name] NVARCHAR(32) NOT NULL
);

CREATE TABLE total (
  [total_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [count] INT,
  [timestamp] DATETIME
);

CREATE TABLE profile (
  [profile_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [identifier] NVARCHAR(32) NOT NULL,
  [first_name] NVARCHAR(64) NOT NULL,
  [middle_name] NVARCHAR(64),
  [last_name] NVARCHAR(64) NOT NULL,
  [gender] NVARCHAR(1) NOT NULL,
  [email] NVARCHAR(128) NOT NULL,
  [birthdate] DATE NOT NULL,
  [status] BIT NOT NULL,
  [claimed] BIT,
  [locked] BIT,
  [mfa_method] NVARCHAR(8)
);

CREATE TABLE link (
  [link_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [profile_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES profile(profile_id),
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [service_identifier] NVARCHAR(50) NOT NULL,
  [first_name] NVARCHAR(50) NOT NULL,
  [last_name] NVARCHAR(50) NOT NULL,
  [address] NVARCHAR(50),
  [created_date] DATETIME NOT NULL,
  [active] BIT NOT NULL,
  [org_unit_path] NVARCHAR(100) NOT NULL,
  [photo_url] NVARCHAR(128),
  [phone] NVARCHAR(10),
  [organization] NVARCHAR(100),
  [last_activity] DATETIME
);

CREATE TABLE agent (
  [agent_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [username] NVARCHAR(32) NOT NULL,
  [password] NVARCHAR(32) NOT NULL
);

CREATE TABLE job (
  [job_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [service_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES service(service_id),
  [start_date] DATETIME NOT NULL,
  [next_runtime] DATETIME NOT NULL,
  [frequency] NVARCHAR(10) NOT NULL,
  [active] BIT NOT NULL
);

CREATE TABLE task (
  [task_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [job_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES job(job_id),
  [start_time] DATETIME NOT NULL,
  [end_time] DATETIME NOT NULL,
  [notes] NVARCHAR(100) NOT NULL,
  [active] BIT NOT NULL
);

CREATE TABLE org (
  [sourced_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [status] BIT NOT NULL,
  [date_last_modified] DATETIME NOT NULL,
  [address] NVARCHAR(32) NOT NULL,
  [city] NVARCHAR(32) NOT NULL,
  [state] NVARCHAR(16) NOT NULL,
  [zip] NVARCHAR(5) NOT NULL,
  [identifier] NVARCHAR(8) NOT NULL,
  [name] NVARCHAR(32) NOT NULL,
  [type] NVARCHAR(8) NOT NULL,
);

CREATE TABLE class (
  [sourced_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [status] BIT NOT NULL,
  [date_last_modified] DATETIME NOT NULL,
  [title] NVARCHAR(32) NOT NULL,
  [class_type] NVARCHAR(32) NOT NULL,
  [class_code] NVARCHAR(16) NOT NULL,
  [location] NVARCHAR(5) NOT NULL,
  [org_sourced_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES org(sourced_id),
);

CREATE TABLE enrollment (
  [sourced_id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
  [status] BIT NOT NULL,
  [date_last_modified] DATETIME NOT NULL,
  [role] NVARCHAR(8) NOT NULL,
  [primary] BIT NOT NULL,
  [type] NVARCHAR(16) NOT NULL,
  [begin_date] NVARCHAR(5) NOT NULL,
  [end_date] NVARCHAR(8) NOT NULL,
  [user_sourced_id] NVARCHAR(50),
  [class_sourced_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES class(sourced_id),
  [org_sourced_id] UNIQUEIDENTIFIER FOREIGN KEY REFERENCES org(sourced_id),
);

SELECT * FROM [task]