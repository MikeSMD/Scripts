CREATE TABLE IF NOT EXISTS File (
    current_dir VARCHAR(150),
    name VARCHAR(150),
    path VARCHAR(150),
    date_added DATE NULL,
    counter INT NULL,
    PRIMARY KEY (current_dir, name)
);

