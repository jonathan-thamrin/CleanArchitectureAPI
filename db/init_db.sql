CREATE TABLE "GroceryItems" (
    "GroceryItem_Id" SERIAL PRIMARY KEY,
    "Name" text NOT NULL,
    "IsComplete" boolean NOT NULL,
    "Author" text NULL
);

INSERT INTO "GroceryItems"("Name", "IsComplete", "Author") VALUES
('Eggs', false, 'Bob'),
('Bread', true, 'Bob'),
('Butter', false, 'Bob');
