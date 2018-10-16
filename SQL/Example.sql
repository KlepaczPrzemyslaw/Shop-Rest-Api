-- Use

USE OnlineShop

-- Insert

	-- User
INSERT INTO Users VALUES ('32ea7a3f-098f-4759-813e-264c5be0ca9e', 0, 'Administrator Piotr', 'admin@admin.com', 'admin', '2018-08-02 12:12:00');
INSERT INTO Users VALUES ('fa434a58-a685-46c7-84ac-8fb98e2aab17', 1, 'User Mateusz', 'user@user.com', 'user', '2018-08-03 10:45:12');
INSERT INTO Users VALUES ('a45f0360-f9a1-4d5b-af69-e5b447505b17', 1, 'User Marek', 'user2@user.com', 'user', '2018-08-04 08:23:14');

	-- Products
INSERT INTO Products VALUES ('c9894032-4142-4736-bba1-ff28dee5e03d', 'Straszny Film', 'Horror', GETDATE(), GETDATE());
INSERT INTO Products VALUES ('ed24912e-0c82-41ec-a003-d937ed794124', 'Film Komediowy', 'Komedia', GETDATE(), GETDATE());
INSERT INTO Products VALUES ('ac1ade76-9313-4c1e-83d1-bcb35130982a', 'Koszulka', 'Zielona - Rozmiar: S', GETDATE(), GETDATE());
INSERT INTO Products VALUES ('5d3d30c1-189a-4f6e-bb39-ed7dfbdd6338', 'Spodnie', 'Niebieskie - Rozmiar: M', GETDATE(), GETDATE());

	-- SingleProductCopy
INSERT INTO SingleProductCopy VALUES ('93c22961-07cd-41a7-8eb2-a94a4db12fc6', 'c9894032-4142-4736-bba1-ff28dee5e03d', 10.50, null, null, null);
INSERT INTO SingleProductCopy VALUES ('35399bb1-9283-4759-bcf4-f78cd84a3696', 'c9894032-4142-4736-bba1-ff28dee5e03d', 10.50, null, null, null);
INSERT INTO SingleProductCopy VALUES ('ae8cae5d-5bb4-43d1-8b24-1bfb1953b286', 'c9894032-4142-4736-bba1-ff28dee5e03d', 10.50, null, null, null);

INSERT INTO SingleProductCopy VALUES ('9bb03400-7b22-4d57-a2ed-ad44f980eea7', 'ed24912e-0c82-41ec-a003-d937ed794124', 30.25, null, null, null);

INSERT INTO SingleProductCopy VALUES ('6f927128-265b-414e-b2dd-b71b33ba3c64', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, 'fa434a58-a685-46c7-84ac-8fb98e2aab17', 'User Mateusz', GETDATE());
INSERT INTO SingleProductCopy VALUES ('c0ccbef4-e506-4db9-9bef-f5084a57cef0', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, 'fa434a58-a685-46c7-84ac-8fb98e2aab17', 'User Mateusz', GETDATE());
INSERT INTO SingleProductCopy VALUES ('28051daa-8da0-437c-b546-b312f6454275', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, null, null, null);
INSERT INTO SingleProductCopy VALUES ('9f7dba24-72b1-46b2-9bdd-5af090055144', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, null, null, null);
INSERT INTO SingleProductCopy VALUES ('abe1bced-a417-4d5b-9d94-6162d0fadcea', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, null, null, null);
INSERT INTO SingleProductCopy VALUES ('4da0358a-4ff3-47fb-826c-99c9c14c8e46', 'ac1ade76-9313-4c1e-83d1-bcb35130982a', 12.75, null, null, null);

INSERT INTO SingleProductCopy VALUES ('f17b9d6b-2809-4c01-bd7c-96b699f1cc71', '5d3d30c1-189a-4f6e-bb39-ed7dfbdd6338', 60.15, 'fa434a58-a685-46c7-84ac-8fb98e2aab17', 'User Mateusz', GETDATE());
INSERT INTO SingleProductCopy VALUES ('da301d12-a0cc-4c72-a282-bce592a66be4', '5d3d30c1-189a-4f6e-bb39-ed7dfbdd6338', 60.15, 'a45f0360-f9a1-4d5b-af69-e5b447505b17', 'User Marek', GETDATE());
INSERT INTO SingleProductCopy VALUES ('b275259b-065c-4349-b805-1dc122ff7583', '5d3d30c1-189a-4f6e-bb39-ed7dfbdd6338', 60.15, null, null, null);
