PGDMP                         z           software-engineering-database    14.0    14.0                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    57552    software-engineering-database    DATABASE     z   CREATE DATABASE "software-engineering-database" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Greek_Greece.1253';
 /   DROP DATABASE "software-engineering-database";
                postgres    false            �            1259    57631    meeting    TABLE     �  CREATE TABLE public.meeting (
    professor character varying(30) NOT NULL,
    student character varying(30) NOT NULL,
    type character varying(9) NOT NULL,
    duration character varying,
    title character varying,
    meet_date timestamp with time zone,
    CONSTRAINT meeting_type_check CHECK (((type)::text = ANY (ARRAY[('online'::character varying)::text, ('in-person'::character varying)::text])))
);
    DROP TABLE public.meeting;
       public         heap    postgres    false            �            1259    57637 	   professor    TABLE     �   CREATE TABLE public.professor (
    professor character varying(30) NOT NULL,
    office_address character varying(45) NOT NULL,
    technology character varying(45) NOT NULL,
    language character varying(45) NOT NULL
);
    DROP TABLE public.professor;
       public         heap    postgres    false            �            1259    57640    student    TABLE     �   CREATE TABLE public.student (
    student character varying(30) NOT NULL,
    start_year numeric(4,0) NOT NULL,
    professor character varying(30) NOT NULL,
    has_ever_connected boolean DEFAULT false NOT NULL
);
    DROP TABLE public.student;
       public         heap    postgres    false            �            1259    57646    thesis    TABLE     Q  CREATE TABLE public.thesis (
    professor character varying(30) NOT NULL,
    student character varying(30) NOT NULL,
    title character varying(45),
    thesis_start_date date,
    grade numeric(2,0),
    language character varying(45),
    technology character varying(45),
    upload1 bytea,
    upload2 bytea,
    upload3 bytea
);
    DROP TABLE public.thesis;
       public         heap    postgres    false            �            1259    57649    users    TABLE       CREATE TABLE public.users (
    username character varying(30) NOT NULL,
    password character varying(30) NOT NULL,
    first_name character varying(30) NOT NULL,
    last_name character varying(30) NOT NULL,
    gender character varying(6) NOT NULL,
    email text NOT NULL,
    phone numeric(10,0) NOT NULL,
    role character varying(9) NOT NULL,
    CONSTRAINT users_email_check CHECK (("position"(email, ' '::text) = 0)),
    CONSTRAINT users_first_name_check CHECK (("position"((first_name)::text, ' '::text) = 0)),
    CONSTRAINT users_gender_check CHECK (((gender)::text = ANY (ARRAY[('male'::character varying)::text, ('female'::character varying)::text, ('other'::character varying)::text]))),
    CONSTRAINT users_password_check CHECK (("position"((password)::text, ' '::text) = 0)),
    CONSTRAINT users_role_check CHECK (((role)::text = ANY (ARRAY[('student'::character varying)::text, ('professor'::character varying)::text]))),
    CONSTRAINT users_username_check CHECK (("position"((username)::text, ' '::text) = 0))
);
    DROP TABLE public.users;
       public         heap    postgres    false                      0    57631    meeting 
   TABLE DATA           W   COPY public.meeting (professor, student, type, duration, title, meet_date) FROM stdin;
    public          postgres    false    209   L'                 0    57637 	   professor 
   TABLE DATA           T   COPY public.professor (professor, office_address, technology, language) FROM stdin;
    public          postgres    false    210   !(                 0    57640    student 
   TABLE DATA           U   COPY public.student (student, start_year, professor, has_ever_connected) FROM stdin;
    public          postgres    false    211   n(                 0    57646    thesis 
   TABLE DATA           �   COPY public.thesis (professor, student, title, thesis_start_date, grade, language, technology, upload1, upload2, upload3) FROM stdin;
    public          postgres    false    212   �(                 0    57649    users 
   TABLE DATA           f   COPY public.users (username, password, first_name, last_name, gender, email, phone, role) FROM stdin;
    public          postgres    false    213   �)       t           2606    57661    professor professor_pkey 
   CONSTRAINT     ]   ALTER TABLE ONLY public.professor
    ADD CONSTRAINT professor_pkey PRIMARY KEY (professor);
 B   ALTER TABLE ONLY public.professor DROP CONSTRAINT professor_pkey;
       public            postgres    false    210            v           2606    57663    student student_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_pkey PRIMARY KEY (student);
 >   ALTER TABLE ONLY public.student DROP CONSTRAINT student_pkey;
       public            postgres    false    211            x           2606    57665    thesis thesis_pkey 
   CONSTRAINT     U   ALTER TABLE ONLY public.thesis
    ADD CONSTRAINT thesis_pkey PRIMARY KEY (student);
 <   ALTER TABLE ONLY public.thesis DROP CONSTRAINT thesis_pkey;
       public            postgres    false    212            z           2606    57667    thesis thesis_title_key 
   CONSTRAINT     S   ALTER TABLE ONLY public.thesis
    ADD CONSTRAINT thesis_title_key UNIQUE (title);
 A   ALTER TABLE ONLY public.thesis DROP CONSTRAINT thesis_title_key;
       public            postgres    false    212            |           2606    57669    users users_email_key 
   CONSTRAINT     Q   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);
 ?   ALTER TABLE ONLY public.users DROP CONSTRAINT users_email_key;
       public            postgres    false    213            ~           2606    57671    users users_phone_key 
   CONSTRAINT     Q   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_phone_key UNIQUE (phone);
 ?   ALTER TABLE ONLY public.users DROP CONSTRAINT users_phone_key;
       public            postgres    false    213            �           2606    57673    users users_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (username);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    213            �           2606    57674    meeting meeting_professor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.meeting
    ADD CONSTRAINT meeting_professor_fkey FOREIGN KEY (professor) REFERENCES public.users(username) ON DELETE CASCADE;
 H   ALTER TABLE ONLY public.meeting DROP CONSTRAINT meeting_professor_fkey;
       public          postgres    false    3200    209    213            �           2606    57679    meeting meeting_student_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.meeting
    ADD CONSTRAINT meeting_student_fkey FOREIGN KEY (student) REFERENCES public.users(username) ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.meeting DROP CONSTRAINT meeting_student_fkey;
       public          postgres    false    209    3200    213            �           2606    57684 "   professor professor_professor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.professor
    ADD CONSTRAINT professor_professor_fkey FOREIGN KEY (professor) REFERENCES public.users(username) ON DELETE CASCADE;
 L   ALTER TABLE ONLY public.professor DROP CONSTRAINT professor_professor_fkey;
       public          postgres    false    213    210    3200            �           2606    57689    student student_professor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_professor_fkey FOREIGN KEY (professor) REFERENCES public.professor(professor) ON DELETE CASCADE;
 H   ALTER TABLE ONLY public.student DROP CONSTRAINT student_professor_fkey;
       public          postgres    false    210    211    3188            �           2606    57694    student student_student_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.student
    ADD CONSTRAINT student_student_fkey FOREIGN KEY (student) REFERENCES public.users(username) ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.student DROP CONSTRAINT student_student_fkey;
       public          postgres    false    3200    211    213            �           2606    57699    thesis thesis_professor_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.thesis
    ADD CONSTRAINT thesis_professor_fkey FOREIGN KEY (professor) REFERENCES public.users(username) ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.thesis DROP CONSTRAINT thesis_professor_fkey;
       public          postgres    false    213    3200    212            �           2606    57704    thesis thesis_student_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.thesis
    ADD CONSTRAINT thesis_student_fkey FOREIGN KEY (student) REFERENCES public.users(username) ON DELETE CASCADE;
 D   ALTER TABLE ONLY public.thesis DROP CONSTRAINT thesis_student_fkey;
       public          postgres    false    3200    212    213               �   x��ѽ
�0����٥r����)\����6�Iɥ�oo�"m���)9~��u�Y�y���p�l,�����KP�`�	4j�b�R�HׄuN+̒n�u�d���F�.��4rt����]8�_J���@<ӊ�i�^��8�=!h����J��B���6���u����B��x���&�Î�1o�S��ٮ�$���-         =   x�+(�OK-.�/�410�LV�L�K��,��*�IeL8��Ss�r*93�R2�b���� y9_         N   x�+0�40��420��,(�OK-.�/�L�*.)MI�+J!I��$�2�#��DLc�@`3�����=... ��+�         �   x�}��n�0���H9�Z�ʱR/���*/���;����rk+�aW#=�4��c3&YS�4pt�%�42)%$	���~f~Ŵ�!x4�d�]P��f�'�g^x��}^ؼ7os��wi�Q)��P��3�����ɛq�ܵ6��]vG�Y������Z4�GyѤZM���S0���h�B�j���2r�ߐ��JWű<ZŅھ�./;�{Κ�,˾ �}e�           x�}��N�0Eד���G޹���DY��\š�>IҪ fd{�9s�?��c�pF��P@�n���Ǯ�P���@Q��i�g*C�^˳���!{��Kvz=>f��!;yRkєmO�j��cۑz�X
�H0�!��OIU���R\tcT��1���3+�፴�V�sv`w(���l��T�(�Q2M�HpFa#>}w�An�u��Fzʕ��&�-����џ�.W9��s�췱���
������'bZލ�7�7���{     