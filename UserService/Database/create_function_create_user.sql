CREATE OR REPLACE FUNCTION create_user(_login VARCHAR, _password VARCHAR, _name VARCHAR, _surname VARCHAR, _age INT) 
RETURNS INT AS $$
DECLARE
    new_user_id INT;
BEGIN
    INSERT INTO users (login, password, name, surname, age)
    VALUES (_login, _password, _name, _surname, _age)
    RETURNING id INTO new_user_id;
    RETURN new_user_id;
END;
$$ LANGUAGE plpgsql;
