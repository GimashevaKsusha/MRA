CREATE TABLE public.playlists (
    id integer NOT NULL,
    room_id integer NOT NULL,
    song_id integer NOT NULL,
    number integer NOT NULL,
	UNIQUE (room_id, number)
);


ALTER TABLE public.playlists OWNER TO postgres;


CREATE SEQUENCE public.playlists_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.playlists_id_seq OWNER TO postgres;


ALTER SEQUENCE public.playlists_id_seq OWNED BY public.playlists.id;


CREATE TABLE public.rooms (
    id integer NOT NULL,
    uuid uuid NOT NULL,
    is_active boolean NOT NULL
);


ALTER TABLE public.rooms OWNER TO postgres;


CREATE SEQUENCE public.rooms_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.rooms_id_seq OWNER TO postgres;


ALTER SEQUENCE public.rooms_id_seq OWNED BY public.rooms.id;


CREATE TABLE public.songs (
    id integer NOT NULL,
    title text NOT NULL,
    length time without time zone NOT NULL
);


ALTER TABLE public.songs OWNER TO postgres;


CREATE SEQUENCE public.songs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.songs_id_seq OWNER TO postgres;


ALTER SEQUENCE public.songs_id_seq OWNED BY public.songs.id;


ALTER TABLE ONLY public.playlists ALTER COLUMN id SET DEFAULT nextval('public.playlists_id_seq'::regclass);


ALTER TABLE ONLY public.rooms ALTER COLUMN id SET DEFAULT nextval('public.rooms_id_seq'::regclass);


ALTER TABLE ONLY public.songs ALTER COLUMN id SET DEFAULT nextval('public.songs_id_seq'::regclass);


SELECT pg_catalog.setval('public.playlists_id_seq', 1, false);


SELECT pg_catalog.setval('public.rooms_id_seq', 1, false);


SELECT pg_catalog.setval('public.songs_id_seq', 1, false);


ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_pkey PRIMARY KEY (id);


ALTER TABLE ONLY public.rooms
    ADD CONSTRAINT rooms_pkey PRIMARY KEY (id);


ALTER TABLE ONLY public.songs
    ADD CONSTRAINT songs_pkey PRIMARY KEY (id);


ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_rooms FOREIGN KEY (room_id) REFERENCES public.rooms(id) ON UPDATE CASCADE ON DELETE CASCADE;


ALTER TABLE ONLY public.playlists
    ADD CONSTRAINT playlists_songs FOREIGN KEY (song_id) REFERENCES public.songs(id) ON UPDATE CASCADE ON DELETE CASCADE;