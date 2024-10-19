CREATE OR REPLACE FUNCTION get_user_by_id(_id INT)
RETURNS TABLE (id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS $$
BEGIN
    SELECT users.id, users.login, users.password, users.name, users.surname, users.age
    INTO id, login, password, name, surname, age
    FROM users
    WHERE users.id = _id;
END;
$$ LANGUAGE plpgsql;
