CREATE OR REPLACE FUNCTION delete_user(_id INT)
RETURNS BOOLEAN AS $$
BEGIN
    DELETE FROM users WHERE id = _id;

    IF FOUND THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;
END;
$$ LANGUAGE plpgsql;
