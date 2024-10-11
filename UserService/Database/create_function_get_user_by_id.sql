CREATE OR REPLACE FUNCTION get_user_by_id(_id INT)
RETURNS TABLE (id INT, login VARCHAR, password VARCHAR, name VARCHAR, surname VARCHAR, age INT) AS $$
BEGIN
    RETURN QUERY SELECT * FROM users WHERE id = _id;
END;
$$ LANGUAGE plpgsql;
