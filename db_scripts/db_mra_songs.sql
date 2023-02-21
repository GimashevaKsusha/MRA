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


INSERT INTO public.songs VALUES (1, 'BIBI - BIBI Vengeance', '00:02:45');
INSERT INTO public.songs VALUES (2, 'RM - Wild Flower (with youjeen)', '00:04:33');
INSERT INTO public.songs VALUES (3, 'Jung Kook - Dreamers', '00:03:21');
INSERT INTO public.songs VALUES (4, 'MAMAMOO - ILLELLA', '00:02:46');
INSERT INTO public.songs VALUES (5, 'JIN - The Astronaut', '00:04:42');
INSERT INTO public.songs VALUES (6, 'NewJeans - Attention', '00:03:00');
INSERT INTO public.songs VALUES (7, 'Crush, j-hope - Rush Hour', '00:02:57');
INSERT INTO public.songs VALUES (8, 'TAEYANG, Jimin - VIBE', '00:02:55');
INSERT INTO public.songs VALUES (9, 'TOMORROW X TOGETHER - Sugar Rush Ride', '00:03:06');
INSERT INTO public.songs VALUES (10, 'RM - Lonely', '00:02:46');
INSERT INTO public.songs VALUES (11, 'SEVENTEEN - _WORLD', '00:02:58');
INSERT INTO public.songs VALUES (12, 'NAYEON - POP!', '00:02:48');
INSERT INTO public.songs VALUES (13, 'BTS - Run BTS', '00:03:24');
INSERT INTO public.songs VALUES (14, 'LE SSERAFIM - FEARLESS', '00:02:48');
INSERT INTO public.songs VALUES (15, 'j-hope - MORE', '00:03:00');
INSERT INTO public.songs VALUES (16, '(G)I-DLE - Nxde', '00:02:58');
INSERT INTO public.songs VALUES (17, 'BTS - Yet To Come', '00:03:13');
INSERT INTO public.songs VALUES (18, 'PSY, SUGA - That That', '00:02:54');
INSERT INTO public.songs VALUES (19, 'Jessi - ZOOM', '00:02:54');
INSERT INTO public.songs VALUES (20, 'GOT7 - NANANA', '00:03:07');
INSERT INTO public.songs VALUES (21, 'Stray Kids - CASE 143', '00:03:11');
INSERT INTO public.songs VALUES (22, 'Jung Kook - Stay Alive', '00:03:30');
INSERT INTO public.songs VALUES (23, 'Solar - HONEY', '00:02:47');
INSERT INTO public.songs VALUES (24, 'SEVENTEEN - Darl+ing', '00:02:56');
INSERT INTO public.songs VALUES (25, 'TWICE - Talk that Talk', '00:02:57');


SELECT pg_catalog.setval('public.playlists_id_seq', 1, false);


SELECT pg_catalog.setval('public.rooms_id_seq', 1, false);


SELECT pg_catalog.setval('public.songs_id_seq', 28, true);


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